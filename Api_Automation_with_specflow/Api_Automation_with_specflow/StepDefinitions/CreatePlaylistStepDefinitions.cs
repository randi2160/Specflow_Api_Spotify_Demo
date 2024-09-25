using Api_Automation_with_specflow.utils;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Text;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Infrastructure;

namespace Api_Automation_with_specflow.StepDefinitions
{
    [Binding]
    public class CreatePlaylistStepDefinitions
    {


        private readonly TokenManager _tokenManager;
        private readonly spotifyApiClient _spotifyApiClient;
        private readonly ISpecFlowOutputHelper _outputHelper;
        private HttpResponseMessage _response;
        private string _responseBody;
        private string _token;

        private const string BaseUrl = "https://api.spotify.com"; // Base URL for Spotify API

        public CreatePlaylistStepDefinitions(TokenManager tokenManager, spotifyApiClient spotifyApiClient, ISpecFlowOutputHelper outputHelper)
        {
            _spotifyApiClient = spotifyApiClient;
            _tokenManager = tokenManager;
            _outputHelper = outputHelper;
        }

        [Given(@"User wants to create a playlist")]
        public async Task GivenUserWantsToCreateAPlaylist()
        {
            _outputHelper.WriteLine("Fetching authorization token to create a playlist...");

            // Ensure token is acquired
            _token = await _tokenManager.GetTokenAsync();
            _outputHelper.WriteLine($"Authorization token retrieved: {_token}");
        }

        [When(@"User sends a playlist creation request to endpoint ""(.*)""")]
        public void WhenUserSendsAPlaylistRequestToEndpoint(string endpoint)
        {
            // Build full URL by combining base URL and endpoint
            string fullUrl = $"{BaseUrl}{endpoint}";
            _outputHelper.WriteLine($"Full URL for the playlist creation request: {fullUrl}");
        }

        [When(@"the request body contains:")]
        public async Task WhenTheRequestBodyContains(Table table)
        {
            _outputHelper.WriteLine("Preparing the request body for the playlist creation...");

            var playlistDetails = new
            {
                name = table.Rows[0]["name"],
                description = table.Rows[0]["description"],
                @public = bool.Parse(table.Rows[0]["public"])
            };

            string jsonBody = JsonConvert.SerializeObject(playlistDetails);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            _outputHelper.WriteLine($"Request Body: {jsonBody}");

            // Send the POST request to the endpoint with token
            _outputHelper.WriteLine("Sending the playlist creation request...");
            _response = await _spotifyApiClient.CreatePlaylistAsync($"{BaseUrl}/v1/users/31blole434cqahr3jzhoygkatahi/playlists", content, _token); // Pass the token here
            _responseBody = await _spotifyApiClient.GetResponseBodyAsync(_response);

            _outputHelper.WriteLine("Playlist creation request sent.");
            _outputHelper.WriteLine($"Response received: {_responseBody}");
        }

        [Then(@"the playlist creation is successful with a (.*) status code")]
        public void ThenThePlaylistCreationIsSuccessfulWithAStatusCode(int expectedStatusCode)
        {
            _outputHelper.WriteLine("Validating the status code of the playlist creation response...");

            int actualStatusCode = (int)_response.StatusCode;
            Assert.AreEqual(expectedStatusCode, actualStatusCode, $"Expected status code {expectedStatusCode}, but got {actualStatusCode}.");

            _outputHelper.WriteLine($"Expected status code {expectedStatusCode} received.");
        }

        [Then(@"the response contains the playlist details")]
        public void ThenTheResponseContainsThePlaylistDetails()
        {
            _outputHelper.WriteLine("Validating the playlist details in the response...");

            JObject jsonResponse = JObject.Parse(_responseBody);

            // Validate basic fields returned in the response
            Assert.IsNotNull(jsonResponse["id"], "Playlist ID is missing in the response.");
            Assert.IsNotNull(jsonResponse["name"], "Playlist name is missing in the response.");
            Assert.IsNotNull(jsonResponse["external_urls"], "Playlist external URL is missing in the response.");

            _outputHelper.WriteLine($"Playlist ID: {jsonResponse["id"]}");
            _outputHelper.WriteLine($"Playlist Name: {jsonResponse["name"]}");
            _outputHelper.WriteLine($"Playlist URL: {jsonResponse["external_urls"]["spotify"]}");

            _outputHelper.WriteLine("Playlist details validated successfully.");
        }
    }
}
