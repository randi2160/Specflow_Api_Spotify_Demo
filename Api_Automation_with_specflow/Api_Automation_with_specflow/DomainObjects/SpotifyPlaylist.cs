using System;
using System.Collections.Generic;

public class SpotifyPlaylist
{
    public bool Collaborative { get; set; }
    public string Description { get; set; }
    public ExternalUrls ExternalUrls { get; set; }
    public Followers Followers { get; set; }
    public string Href { get; set; }
    public string Id { get; set; }
    public List<Image> Images { get; set; }
    public object PrimaryColor { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Uri { get; set; }
    public Owner Owner { get; set; }
    public bool Public { get; set; }
    public string SnapshotId { get; set; }
    public Tracks Tracks { get; set; }
}

public class ExternalUrls
{
    public string Spotify { get; set; }
}

public class Followers
{
    public object Href { get; set; }
    public int Total { get; set; }
}

public class Image
{
    // Image details can be added here if needed, currently empty in your JSON
}

public class Owner
{
    public string Href { get; set; }
    public string Id { get; set; }
    public string Type { get; set; }
    public string Uri { get; set; }
    public string DisplayName { get; set; }
    public ExternalUrls ExternalUrls { get; set; }
}

public class Tracks
{
    public int Limit { get; set; }
    public object Next { get; set; }
    public int Offset { get; set; }
    public object Previous { get; set; }
    public string Href { get; set; }
    public int Total { get; set; }
    public List<object> Items { get; set; } // Change object to appropriate type if you have track details
}
