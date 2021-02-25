using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ImageGallerySearch.WebApi.Interfaces;
using ImageGallerySearch.WebApi.Models;
using ImageGallerySearch.WebApi.Models.External;
using Microsoft.Extensions.Configuration;

namespace ImageGallerySearch.WebApi.Services
{
    public class ImageGalleryClient : IImageGalleryClient
    {
        private readonly string _authUrl;
        private readonly string _getImagesUrl;
        private readonly string _apiKey;

        private readonly HttpClient _httpClient;

        public ImageGalleryClient(IConfiguration configuration)
        {
            var settings = configuration.GetSection("ImageGallerySettings").Get<ImageGallerySettings>();

            _authUrl = settings.AuthUrl;
            _getImagesUrl = settings.GetImagesUrl;
            _apiKey = settings.ApiKey;

            _httpClient = new HttpClient();
        }

        public async Task<GetImagesPaginatedResult> GetImagesByPage(int pageNumber)
        {
            var request = await ExecuteRequest(HttpMethod.Get, new Uri($"{_getImagesUrl}/?page={pageNumber}"));

            return await request.Content.ReadFromJsonAsync<GetImagesPaginatedResult>();
        }

        public async Task<GetImageByIdResult> GetImageById(string id)
        {
            var request = await ExecuteRequest(HttpMethod.Get, new Uri($"{_getImagesUrl}/{id}"));
            
            return await request.Content.ReadFromJsonAsync<GetImageByIdResult>();
        }

        private async Task<HttpResponseMessage> ExecuteRequest(HttpMethod method, Uri requestUri)
        {
            if (_httpClient.DefaultRequestHeaders.Authorization == null)
            {
                await RefreshToken();
            }
            
            var request = await _httpClient.SendAsync(new HttpRequestMessage(method, requestUri));

            if (request.StatusCode == HttpStatusCode.Unauthorized)
            {
                await RefreshToken();
                
                return await _httpClient.SendAsync(new HttpRequestMessage(method, requestUri));
            }

            if (!request.IsSuccessStatusCode)
            {
                throw new Exception($"{method.Method} request to {requestUri} failed: {request.ReasonPhrase}.");
            }

            return request;
        }

        private async Task RefreshToken()
        {
            var body = new {apiKey = _apiKey};
            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            var result = await _httpClient.PostAsync(_authUrl, content);

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception("Unable to authenticate with the given apiKey.");
            }

            var authenticationResult = await result.Content.ReadFromJsonAsync<AuthenticationResult>();

            // TODO: is this necessary?
            if (authenticationResult == null)
            {
                throw new Exception("An error occurred reading the token when authenticating.");
            }

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", authenticationResult.Token);
        }
    }
}