using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;
using NotTinderAppMAUI.Helprers;
using NotTinderAppMAUI.Models.Authenticate;
using NotTinderAppMAUI.Models.DTOs;
using NotTinderAppMAUI.Models.Register;

namespace NotTinderAppMAUI.Models;

public class ApiServiceProvider
{
    private const string ACCESS_TOKEN = "access_token";
    private const string REFRESH_TOKEN = "refresh_token";

    private readonly HttpsConnectionHelper _httpsConnection;

    private string _accessToken;
    private string _refreshToken;

    public ApiServiceProvider(HttpsConnectionHelper httpsConnection)
    {
        _httpsConnection = httpsConnection;
    }

    private async Task<string> GetSecureTokenAsync(string key)
    {
        return await SecureStorage.Default.GetAsync(key);
    }


    public async Task<ResponseDto<LoginData>> Authenticate(AuthenticateRequest request)
    {
        var httpRequestMessage = new HttpRequestMessage();
        httpRequestMessage.Method = HttpMethod.Post;
        httpRequestMessage.RequestUri = new Uri(_httpsConnection.DevServerRootUrl + "/api/Login");

        if (request != null)
        {
            var jsonContent = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            httpRequestMessage.Content = httpContent;
        }

        try
        {
            var response = await _httpsConnection.HttpClient.SendAsync(httpRequestMessage);
            var responseContent = await response.Content.ReadAsStringAsync();

            LoginData result = default;

            if (response.IsSuccessStatusCode)
            {
                result = JsonConvert.DeserializeObject<LoginData>(responseContent);
                await SecureStorage.Default.SetAsync(ACCESS_TOKEN, result.Tokens.AccessToken);
                await SecureStorage.Default.SetAsync(REFRESH_TOKEN, result.Tokens.RefreshToken);
            }

            return new ResponseDto<LoginData>
            {
                IsSuccess = response.IsSuccessStatusCode,
                Message = response.IsSuccessStatusCode ? string.Empty : responseContent,
                Data = result
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<LoginData>
            {
                IsSuccess = false,
                Message = "Happen something bad"
            };
        }
    }

    private async Task<ResponseDto> RefreshToken(TokenDto tokenData)
    {
        var httpRequestMessage = new HttpRequestMessage();
        httpRequestMessage.Method = HttpMethod.Post;
        httpRequestMessage.RequestUri = new Uri(_httpsConnection.DevServerRootUrl + "/api/RefreshToken");

        if (tokenData != null)
        {
            var jsonContent = JsonConvert.SerializeObject(tokenData);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            httpRequestMessage.Content = httpContent;
        }

        try
        {
            var response = await _httpsConnection.HttpClient.SendAsync(httpRequestMessage);
            var responseContent = await response.Content.ReadAsStringAsync();

            TokenDto result = default;


            if (response.IsSuccessStatusCode)
            {
                result = JsonConvert.DeserializeObject<TokenDto>(responseContent);
                await SecureStorage.Default.SetAsync(ACCESS_TOKEN, result.AccessToken);
                await SecureStorage.Default.SetAsync(REFRESH_TOKEN, result.RefreshToken);
            }

            return new ResponseDto
            {
                IsSuccess = response.IsSuccessStatusCode,
                Message = response.IsSuccessStatusCode ? string.Empty : responseContent
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Happen something bad"
            };
        }
    }

    public async Task<ResponseDto> Register(RegisterRequest request)
    {
        var httpRequestMessage = new HttpRequestMessage();
        httpRequestMessage.Method = HttpMethod.Post;
        httpRequestMessage.RequestUri = new Uri(_httpsConnection.DevServerRootUrl + "/api/Register");

        var jsonContent = JsonConvert.SerializeObject(request);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        httpRequestMessage.Content = httpContent;


        try
        {
            var response = await _httpsConnection.HttpClient.SendAsync(httpRequestMessage);
            var responseContent = await response.Content.ReadAsStringAsync();

            return new ResponseDto { IsSuccess = response.IsSuccessStatusCode, Message = responseContent };
        }
        catch (Exception e)
        {
            var result = new ResponseDto
            {
                IsSuccess = false,
                Message = "Happen something bad"
            };

            return result;
        }
    }

    public async Task<ResponseDto> LogOut()
    {
        var httpRequestMessage = new HttpRequestMessage();
        httpRequestMessage.Method = HttpMethod.Get;
        httpRequestMessage.RequestUri = new Uri(_httpsConnection.DevServerRootUrl + "/api/LogOut");


        try
        {
            var response = await _httpsConnection.HttpClient.SendAsync(httpRequestMessage);
            var responseContent = await response.Content.ReadAsStringAsync();

            return new ResponseDto { IsSuccess = response.IsSuccessStatusCode, Message = responseContent };
        }
        catch (Exception e)
        {
            var result = new ResponseDto
            {
                IsSuccess = false,
                Message = "Happen something bad"
            };

            return result;
        }
    }

    private async Task<ResponseDto<TResponseData>> ExecuteRequest<TResponseData>(
        HttpRequestMessage httpRequestMessage, bool deserializeResponse = true)
    {
        try
        {
            var response = await _httpsConnection.HttpClient.SendAsync(httpRequestMessage);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var tokens = await RefreshToken(new TokenDto
                {
                    AccessToken = await GetSecureTokenAsync(ACCESS_TOKEN),
                    RefreshToken = await GetSecureTokenAsync(REFRESH_TOKEN)
                });
                if (tokens.IsSuccess)
                {
                    var newRequest =
                        new HttpRequestMessage(httpRequestMessage.Method, httpRequestMessage.RequestUri)
                        {
                            Content = httpRequestMessage.Content
                        };

                    newRequest.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer", await GetSecureTokenAsync(ACCESS_TOKEN));
                    response = await _httpsConnection.HttpClient.SendAsync(newRequest);
                }
            }

            TResponseData data = default;
            var responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                data = deserializeResponse
                    ? JsonConvert.DeserializeObject<TResponseData>(responseContent)
                    : default;

            return new ResponseDto<TResponseData>
            {
                IsSuccess = response.IsSuccessStatusCode,
                Data = data,
                Message = responseContent
            };
        }
        catch (Exception e)
        {
            return new ResponseDto<TResponseData>
            {
                IsSuccess = false,
                Message = "Something went wrong."
            };
        }
    }

    private async Task<ResponseDto<TResponseData>> InternalCallWebApi<TRequest, TResponseData>(
        string apiUrl, IEnumerable<FileDto>? files, HttpMethod method, bool deserializeResponse = true,
        TRequest? request = null) where TRequest : class
    {
        var url = new Uri(_httpsConnection.DevServerRootUrl + apiUrl);
        using var httpRequestMessage = new HttpRequestMessage(method, url);
        httpRequestMessage.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", await GetSecureTokenAsync(ACCESS_TOKEN));

        if (files != null && files.Any())
        {
            var multipartContent = new MultipartFormDataContent();
            foreach (var file in files)
            {
                var fileContent = new StreamContent(file.FileStream)
                {
                    Headers = { ContentType = MediaTypeHeaderValue.Parse(MediaTypeNames.Image.Png) }
                };
                multipartContent.Add(fileContent, file.AtributeName, file.FileName);
            }

            var jsonContent = JsonConvert.SerializeObject(request);
            multipartContent.Add(new StringContent(jsonContent, Encoding.UTF8, "application/json"), "data");
            httpRequestMessage.Content = multipartContent;
        }
        else if (request != null)
        {
            httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8,
                "application/json");
        }

        return await ExecuteRequest<TResponseData>(httpRequestMessage, deserializeResponse);
    }

    public async Task<ResponseDto<TResponseData>> CallWebApi<TRequest, TResponseData>(
        string apiUrl, TRequest request, IEnumerable<FileDto>? files = null, HttpMethod? method = null)
        where TRequest : class
    {
        return await InternalCallWebApi<TRequest, TResponseData>(apiUrl, files, method ?? HttpMethod.Post,
            request: request);
    }

    public async Task<ResponseDto<TResponseData>> CallWebApi<TResponseData>(
        string apiUrl, IEnumerable<FileDto>? files = null, HttpMethod? method = null)
    {
        return await InternalCallWebApi<object, TResponseData>(apiUrl, files, method ?? HttpMethod.Get);
    }

    public async Task<ResponseDto> CallWebApi<TRequest>(
        string apiUrl, TRequest request, IEnumerable<FileDto> files = null, HttpMethod? method = null)
        where TRequest : class
    {
        var response =
            await InternalCallWebApi<TRequest, object>(apiUrl, files, method ?? HttpMethod.Post, false, request);
        return new ResponseDto { IsSuccess = response.IsSuccess, Message = response.Message };
    }

    public async Task<ResponseDto> CallWebApi(
        string apiUrl, IEnumerable<FileDto>? files = null, HttpMethod? method = null)
    {
        var response = await InternalCallWebApi<object, object>(apiUrl, files, method ?? HttpMethod.Get, false);
        return new ResponseDto { IsSuccess = response.IsSuccess, Message = response.Message };
    }
}