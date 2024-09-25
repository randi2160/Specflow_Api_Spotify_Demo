Feature: get playlist

A short summary of the feature
Background:
    Given User has a valid authorization token

@spotify2
Scenario: I want to get PlayList when user is authorized
    Given User hit the following url "https://api.spotify.com/v1/playlists/3cEYpjA9oz9GiPac4AsH4n"

    Then i expect a valid response return with 200 status code
   
    And i expect description to return
    And i expect to see Id to be displayed
    And i expect External URL to be preset
	

@spotify1
Scenario: I want to add Limit of 1 returns only 1 Record
	Given User hit the following url "https://api.spotify.com/v1/playlists/3cEYpjA9oz9GiPac4AsH4n/"
	And add a Query parameter of ?Limit=1  
	Then i expect a valid response return with 200 status code
	#Then i expect only 1 record to be returned
	Then i expect description to return
	Then i expect to see Id to be displayed
	Then i expect External URL to be preset