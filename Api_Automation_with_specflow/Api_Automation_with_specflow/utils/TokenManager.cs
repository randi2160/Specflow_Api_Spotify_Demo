using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api_Automation_with_specflow.utils
{
    public class TokenManager
    {
        private readonly HttpClient _httpClient;
        private readonly ConfigurationService _configService;
        private string _accessToken;
        private DateTime _tokenExpiryTime;
        private static readonly object _lock = new object(); // Ensures thread safety for token management

        public TokenManager(HttpClient httpClient, ConfigurationService configService)
        {
            _httpClient = httpClient;
            _configService = configService;
        }

        // Method to get the token, ensuring it's only renewed if expired
    public async Task<string> GetTokenAsync()
        {
            // Ensure thread safety by locking access to token
            lock (_lock)
            {
                if (!IsTokenExpired() && !string.IsNullOrEmpty(_accessToken))
                {
                    // Token is valid, no need to renew
                    return _accessToken;
                }
            }

            // If token is expired or doesn't exist, renew it
            return await RenewTokenAsync();
        }

        // Method to renew the token
        public async Task<string> RenewTokenAsync()
        {
            lock (_lock)
            {
                // Double check inside the lock to avoid multiple renewals
                if (!IsTokenExpired() && !string.IsNullOrEmpty(_accessToken))
                {
                    return _accessToken;
                }
            }

            var formData = new Dictionary<string, string>
        {
            {"grant_type", "refresh_token"},
            {"refresh_token", _configService.GetRefreshToken()},
            {"client_id", _configService.GetClientId()},
            {"client_secret", _configService.GetClientSecret()}
        };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token")
            {
                Content = new FormUrlEncodedContent(formData)
            };

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseBody);

            lock (_lock) // Ensure thread safety when setting token
            {
                _accessToken = tokenResponse.access_token;
                _tokenExpiryTime = DateTime.UtcNow.AddSeconds(tokenResponse.expires_in);
            }

            return _accessToken;
        }

        private bool IsTokenExpired()
        {
            return DateTime.UtcNow >= _tokenExpiryTime;
        }

        private class TokenResponse
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }
        }

    }
}
