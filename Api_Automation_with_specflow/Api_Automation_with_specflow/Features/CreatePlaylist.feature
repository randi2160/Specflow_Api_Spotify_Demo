Feature: createPlaylist

 Background:
    Given User has a valid authorization token

  @spotify
  Scenario: User is able to Create a Playlist
    Given User wants to create a playlist
    When User sends a playlist creation request to endpoint "/v1/users/31blole434cqahr3jzhoygkatahi/playlists"
    And the request body contains:
      | name        | description              | public |
      | New Playlist| New playlist description | false  |
    Then the playlist creation is successful with a 201 status code
    And the response contains the playlist details
