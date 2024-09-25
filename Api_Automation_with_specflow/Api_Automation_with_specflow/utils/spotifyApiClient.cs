using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api_Automation_with_specflow.utils
{
    public class spotifyApiClient
    {
        private readonly HttpClient _httpClient;

        public spotifyApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Method to create an HttpRequestMessage with an Authorization Header

        private HttpRequestMessage CreateRequestWithToken(string uri, string token, HttpMethod method)
        {
            var request = new HttpRequestMessage(method, uri);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return request;
        }

        // Method to get playlist(GET request)
        public async Task<HttpResponseMessage> GetPlaylistAsync(string uri, string token)
        {
            var request = CreateRequestWithToken(uri, token, HttpMethod.Get);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return response;
        }

        // Method to create a playlist (POST request)
        public async Task<HttpResponseMessage> CreatePlaylistAsync(string uri, HttpContent content, string token)
        {
            var request = CreateRequestWithToken(uri, token, HttpMethod.Post);
            request.Content = content;
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return response;
        }

        // Method to get the response body as a string
        public async Task<string> GetResponseBodyAsync(HttpResponseMessage response)
        {
            return await response.Content.ReadAsStringAsync();
        }

    }
}
