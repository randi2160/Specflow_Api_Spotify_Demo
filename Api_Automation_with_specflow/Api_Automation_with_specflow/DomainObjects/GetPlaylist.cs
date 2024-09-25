using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecFlowProject.DomainObjects
{
    public class GetPlaylist
    {
        public class ExternalUrls
        {
            public string spotify { get; set; }
        }

        public class Followers
        {
            public object href { get; set; }
            public int total { get; set; }
        }

        public class Image
        {
            public object height { get; set; }
            public string url { get; set; }
            public object width { get; set; }
        }

        public class Owner
        {
            public string display_name { get; set; }
            public ExternalUrls external_urls { get; set; }
            public string href { get; set; }
            public string id { get; set; }
            public string type { get; set; }
            public string uri { get; set; }
        }

        public class AddedBy
        {
            public ExternalUrls external_urls { get; set; }
            public string href { get; set; }
            public string id { get; set; }
            public string type { get; set; }
            public string uri { get; set; }
        }

        public class Track
        {
            public string preview_url { get; set; }
            public List<string> available_markets { get; set; }
            public bool explicitContent { get; set; }
            public string type { get; set; }
            public Album album { get; set; }
            public string name { get; set; }
            public bool is_local { get; set; }
        }

        public class Album
        {
            public List<string> available_markets { get; set; }
            public string name { get; set; }
            public string href { get; set; }
        }

        public class Item
        {
            public string added_at { get; set; }
            public AddedBy added_by { get; set; }
            public Track track { get; set; }
        }

        public class Tracks
        {
            public string href { get; set; }
            public List<Item> items { get; set; }
            public int limit { get; set; }
            public object next { get; set; }
            public int offset { get; set; }
            public object previous { get; set; }
            public int total { get; set; }
        }

        public class Playlist
        {
            public bool collaborative { get; set; }
            public string description { get; set; }
            public ExternalUrls external_urls { get; set; }
            public Followers followers { get; set; }
            public string href { get; set; }
            public string id { get; set; }
            public List<Image> images { get; set; }
            public string name { get; set; }
            public Owner owner { get; set; }
            public object primary_color { get; set; }
            public bool publicStatus { get; set; }
            public string snapshot_id { get; set; }
            public Tracks tracks { get; set; }
            public string type { get; set; }
            public string uri { get; set; }
        }
    }
}
