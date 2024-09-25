using Api_Automation_with_specflow.utils;
using Newtonsoft.Json;
using NUnit.Framework;
using SpecFlowProject.DomainObjects;
using SpecFlowProject.utils;
using System.Net.Http;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Infrastructure;

namespace SpecFlowProject.StepDefinitions
{
    [Binding]
    public class SpotifyApiStepDefinitions
    {
        private readonly TokenManager _tokenManager;
        private readonly spotifyApiClient _spotifyApiClient;
        private readonly ISpecFlowOutputHelper _outputHelper;
        private HttpResponseMessage _response;
        private string _responseBody;
        private GetPlaylist.Playlist _playlist;
        private string _token;

        public SpotifyApiStepDefinitions(TokenManager tokenManager, spotifyApiClient spotifyApiClient, ISpecFlowOutputHelper outputHelper)
        {
            _spotifyApiClient = spotifyApiClient;
            _tokenManager = tokenManager;
            _outputHelper = outputHelper;
        }

        [Given(@"User has a valid authorization token")]
        public async Task GivenUserHasAValidAuthorizationToken()
        {
            _token = await _tokenManager.GetTokenAsync(); // Get the token using TokenManager
            _outputHelper.WriteLine($"Using token: {_token}");
            _outputHelper.WriteLine("Authorization header set with a valid token.");
        }

        [Given(@"User hit the following url ""(.*)""")]
        public async Task GivenUserHitTheFollowingUrl(string url)
        {
            _outputHelper.WriteLine($"Hitting the Spotify API endpoint: {url}");

            _response = await _spotifyApiClient.GetPlaylistAsync(url, _token); // Pass token directly into the API method
            _response.EnsureSuccessStatusCode();
            _responseBody = await _spotifyApiClient.GetResponseBodyAsync(_response);

            _playlist = JsonConvert.DeserializeObject<GetPlaylist.Playlist>(_responseBody);

            _outputHelper.WriteLine($"Response received and deserialized: {_responseBody}");
        }

        [Given(@"add a Query parameter of \?Limit=(.*)")]
        public async Task GivenAddAQueryParameterOfLimit(int limit)
        {
            var url = $"https://api.spotify.com/v1/playlists/3cEYpjA9oz9GiPac4AsH4n?limit={limit}";
            _outputHelper.WriteLine($"Final URL: {url}");

            _token = await _tokenManager.GetTokenAsync(); // Ensure token is refreshed if needed

            _response = await _spotifyApiClient.GetPlaylistAsync(url, _token); // Pass token directly into the API method
            _responseBody = await _spotifyApiClient.GetResponseBodyAsync(_response);

            _playlist = JsonConvert.DeserializeObject<GetPlaylist.Playlist>(_responseBody);

            _outputHelper.WriteLine($"Response Body: {_responseBody}");
        }

        [Then(@"i expect a valid response return with (.*) status code")]
        public void ThenIExpectAValidResponseReturnWithStatusCode(int expectedStatusCode)
        {
            var actualStatusCode = (int)_response.StatusCode;
            Assert.AreEqual(expectedStatusCode, actualStatusCode, $"Expected status code is {expectedStatusCode}, but got {actualStatusCode}.");
            _outputHelper.WriteLine($"Actual status code is: {actualStatusCode}");
        }

        [Then(@"i expect description to return")]
        public void ThenIExpectDescriptionToReturn()
        {
            Assert.IsNotNull(_playlist.description, "Description is null.");
            _outputHelper.WriteLine($"Description: {_playlist.description}");
        }

        [Then(@"i expect to see Id to be displayed")]
        public void ThenIExpectToSeeIdToBeDisplayed()
        {
            Assert.IsNotNull(_playlist.id, "Playlist ID is missing.");
            _outputHelper.WriteLine($"Playlist ID: {_playlist.id}");
        }

        [Then(@"i expect External URL to be preset")]
        public void ThenIExpectExternalURLToBePreset()
        {
            Assert.IsNotNull(_playlist.external_urls.spotify, "External URL is missing.");
            _outputHelper.WriteLine($"External URL: {_playlist.external_urls.spotify}");
        }

        [Then(@"Expect that the name, id returned")]
        public void ThenExpectThatTheNameIdReturned()
        {
            Assert.IsNotNull(_playlist.name, "Playlist name should be present in the response.");
            Assert.IsNotNull(_playlist.id, "Playlist ID should be present in the response.");
            _outputHelper.WriteLine($"Playlist Name: {_playlist.name}");
            _outputHelper.WriteLine($"Playlist ID: {_playlist.id}");
        }
    }
}
