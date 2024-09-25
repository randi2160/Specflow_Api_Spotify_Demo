using Microsoft.Extensions.Configuration;
using System.IO;

namespace SpecFlowProject.utils
{
    public class ConfigReader
    {
        private readonly IConfiguration _configuration;

        public ConfigReader()
        {
            // Get the base path dynamically (works even when running from different folders)
            var basePath = Directory.GetCurrentDirectory();

            // Look for appsettings.json in the project root
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _configuration = builder.Build();
        }

        public string GetSpotifyClientId() => _configuration["Spotify:ClientId"];
        public string GetSpotifyClientSecret() => _configuration["Spotify:ClientSecret"];
        public string GetSpotifyRefreshToken() => _configuration["Spotify:RefreshToken"];
        public string GetSpotifyRedirectUri() => _configuration["Spotify:RedirectUri"];
        public string GetSpotifyAuthorizationCode() => _configuration["Spotify:AuthorizationCode"];
    }
}
