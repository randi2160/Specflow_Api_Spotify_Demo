

using SpecFlowProject.utils;

namespace Api_Automation_with_specflow.utils
{
   
    public class ConfigurationService
    {
        private readonly ConfigReader _configReader;

        public ConfigurationService()
        {
            _configReader = new ConfigReader();
        }

        public string GetClientId()
        {
            var clientId = _configReader.GetSpotifyClientId();
            if (string.IsNullOrEmpty(clientId))
            {
                throw new InvalidOperationException("Client ID is not set in appsettings.json.");
            }
            return clientId;
        }

        public string GetClientSecret()
        {
            var clientSecret = _configReader.GetSpotifyClientSecret();
            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new InvalidOperationException("Client Secret is not set in appsettings.json.");
            }
            return clientSecret;
        }

        public string GetRefreshToken()
        {
            var refreshToken = _configReader.GetSpotifyRefreshToken();
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new InvalidOperationException("Refresh Token is not set in appsettings.json.");
            }
            return refreshToken;
        }

        public string GetRedirectUri()
        {
            var redirectUri = _configReader.GetSpotifyRedirectUri();
            if (string.IsNullOrEmpty(redirectUri))
            {
                throw new InvalidOperationException("Redirect URI is not set in appsettings.json.");
            }
            return redirectUri;
        }

        public string GetAuthorizationCode()
        {
            var authCode = _configReader.GetSpotifyAuthorizationCode();
            if (string.IsNullOrEmpty(authCode))
            {
                throw new InvalidOperationException("Authorization Code is not set in appsettings.json.");
            }
            return authCode;
        }
    }
}
