using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using TuyaSharp.Models.Requests;
using TuyaSharp.Utils;

namespace TuyaSharp.Models;

internal class RequestContext<TRequestBody>
{
    private string _signature;
    private HttpRequestMessage? _httpRequestMessage;

    internal RequestContext(
        TuyaSharpClientOptions options,
        string baseUrl,
        string path,
        Request<TRequestBody?> request,
        Token? token,
        JsonSerializerSettings jsonSerializerSettings)
    {
        Options = options;
        BaseUrl = baseUrl;
        Path = path;
        Request = request;
        Token = token;
        JsonSerializerSettings = jsonSerializerSettings;

        _signature = CalculateSignature();
    }

    internal TuyaSharpClientOptions Options { get; }
    internal string BaseUrl { get; }
    internal string Path { get; }
    internal Request<TRequestBody?> Request { get; }
    internal Token? Token { get; }
    internal JsonSerializerSettings JsonSerializerSettings { get; }

    internal string Nonce { get; private set; } = string.Empty;
    internal long Timestamp { get; private set; }
    internal string Signature => string.IsNullOrEmpty(_signature) ? _signature = CalculateSignature() : _signature;

    internal string? JsonContent { get; private set; } = string.Empty;

    internal HttpRequestMessage HttpRequestMessage => _httpRequestMessage ??= CreateHttpRequestMessage();

    private string CalculateSignature()
    {
        string? jsonContent = null;
        byte[] encodedContent;

        if (Request.Body is null) encodedContent = CryptoHelper.Sha256(Encoding.UTF8.GetBytes(string.Empty));
        else
        {
            jsonContent = JsonConvert.SerializeObject(Request.Body, JsonSerializerSettings);
            encodedContent = CryptoHelper.Sha256(Encoding.UTF8.GetBytes(jsonContent));
        }

        JsonContent = jsonContent;
        Nonce = Guid.NewGuid().ToString("N");
        Timestamp = DateTimeOffset.UtcNow.ToTimestamp();

        var stringToSign = StringToSignGenerator.Generate(
            Request.HttpMethod,
            encodedContent.ToHexString().ToLower(),
            Path);

        return SignGenerator.Generate(
            Options.ClientId,
            Options.ClientSecret,
            Timestamp.ToString(),
            Nonce,
            stringToSign,
            Token?.AccessToken ?? null);
    }

    private HttpRequestMessage CreateHttpRequestMessage()
    {
        var requestMessage = new HttpRequestMessage(Request.HttpMethod, BaseUrl + Path)
        {
            Headers =
            {
                {
                    "t", Timestamp.ToString()
                },
                {
                    "nonce", Nonce
                },
                {
                    "sign", Signature
                }
            }
        };

        if (!string.IsNullOrEmpty(Token?.AccessToken))
        {
            requestMessage.Headers.Add("access_token", Token.AccessToken);
        }

        if (!string.IsNullOrEmpty(JsonContent))
        {
            requestMessage.Content = new StringContent(JsonContent, Encoding.UTF8, "application/json");
        }

        return requestMessage;
    }
}
