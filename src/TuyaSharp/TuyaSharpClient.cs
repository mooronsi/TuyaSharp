using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TuyaSharp.Abstractions;
using TuyaSharp.DTO.Token;
using TuyaSharp.Exceptions;
using TuyaSharp.JsonConverters;
using TuyaSharp.Mappers;
using TuyaSharp.Models;
using TuyaSharp.Models.Requests;
using TuyaSharp.Services;

namespace TuyaSharp;

public class TuyaSharpClient : ITuyaSharpClient
{
    private readonly TuyaSharpClientOptions _options;
    private readonly HttpClient _httpClient;

    private readonly IRegionHostResolver _regionHostResolver = new RegionHostResolver();
    private readonly ILanguageResolver _languageResolver = new LanguageResolver();

    private readonly JsonSerializerSettings _jsonSerializerSettings = new()
    {
        Converters = { new CommandJsonConverter() }
    };

    private Token? _token;

    public TuyaSharpClient(
        TuyaSharpClientOptions options,
        HttpClient? httpClient = null,
        CancellationToken cancellationToken = default)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _httpClient = httpClient ?? new HttpClient();
        CancellationToken = cancellationToken;

        ConfigureHttpClient();

        TokenService = new TokenService(this);
        DeviceControl = new DeviceControl(this);
    }

    public CancellationToken CancellationToken { get; }

    public ITokenService TokenService { get; }
    public IDeviceControl DeviceControl { get; }

    public async Task<Response<TResponseBody?>> SendAsync<TRequestBody, TResponseBody>(
        Request<TRequestBody> request,
        bool withoutToken = false,
        CancellationToken cancellationToken = default)
    {
        if (request is null) throw new ArgumentException(nameof(request));
        if (request.HttpMethod is null) throw new ArgumentException(nameof(request.HttpMethod));

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(CancellationToken, cancellationToken);
        cancellationToken = cts.Token;

        try
        {
            if (!withoutToken) await ActualizeToken(cancellationToken).ConfigureAwait(false);

            var baseUrl = $"https://{_regionHostResolver.Resolve(_options.RegionHost)}";
            var path = BuildPath(request.Path, request.QueryParameters);

            var requestContext =
                new RequestContext<TRequestBody>(_options, baseUrl, path, request!, _token, _jsonSerializerSettings);
            var response = await _httpClient.SendAsync(requestContext.HttpRequestMessage, cancellationToken)
                .ConfigureAwait(false);
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            var responseObject =
                JsonConvert.DeserializeObject<Response<TResponseBody?>>(responseContent, _jsonSerializerSettings)
                ?? throw new RequestException("Response deserialization failed.");

            if (!responseObject.Success.HasValue || !responseObject.Success.Value)
            {
                throw new RequestException(
                    $"Request failed. Error code: {responseObject.Code}, message: {responseObject.Message}");
            }

            return responseObject;
        }
        catch (TaskCanceledException ex)
        {
            if (cancellationToken.IsCancellationRequested) throw;
            throw new RequestException("Request timed out.", ex);
        }
        catch (Exception ex)
        {
            throw new RequestException("Exception during making request.", ex);
        }
    }

    private void ConfigureHttpClient()
    {
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "TuyaSharp");
        _httpClient.DefaultRequestHeaders.Add("client_id", _options.ClientId);
        _httpClient.DefaultRequestHeaders.Add("client_secret", _options.ClientSecret);
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        _httpClient.DefaultRequestHeaders.Add("sign_method", "HMAC-SHA256");
        _httpClient.DefaultRequestHeaders.Add("lang", _languageResolver.Resolve(_options.Language));
    }

    private async Task ActualizeToken(CancellationToken cancellationToken = default)
    {
        if (_token is null || string.IsNullOrEmpty(_token.AccessToken) || _token.IsExpired)
        {
            var response = await TokenService.GetTokenAsync(new GetTokenRequest(), cancellationToken);
            _token = (response.Result ?? throw new RequestException("Token response is null.")).Map();
        }

        if (_token.IsExpired)
        {
            var response =
                await TokenService.RefreshTokenAsync(new RefreshTokenRequest(_token.RefreshToken), cancellationToken);
            _token = (response.Result ?? throw new RequestException("Token response is null.")).Map();
        }
    }

    private static string BuildPath(string? path, SortedDictionary<string, string>? queryParameters)
    {
        path ??= string.Empty;

        if (queryParameters is not null && queryParameters.Count != 0)
        {
            path += "?";
            path = queryParameters
                .Aggregate(path, (current, requestArg) =>
                    current + $"&{requestArg.Key}={requestArg.Value}")
                .Replace("?&", "?");
        }

        return path;
    }
}
