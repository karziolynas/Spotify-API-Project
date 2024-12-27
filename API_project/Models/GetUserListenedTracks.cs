using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_project
{

    public class Rootobject
    {
        public string href { get; set; }
        public int limit { get; set; }
        public string next { get; set; }
        public int total { get; set; }
        public Item[] items { get; set; }
    }

    public class Item
    {
        public Track track { get; set; }
        public string played_at { get; set; }
    }

    public class Track
    {
        public Album album { get; set; }
        public Artist[] artists { get; set; }
        public string[] available_markets { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public int track_number { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class Album
    {
        public Image[] images { get; set; }
        public string name { get; set; }
        public string uri { get; set; }
        public Artist[] artists { get; set; }
    }

    public class Image
    {
        public string url { get; set; }
        public int height { get; set; }
        public int width { get; set; }
    }

    public class Artist
    {
        public string href { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }
}