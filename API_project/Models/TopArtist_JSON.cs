namespace API_project.Models
{
    public class TopArtist_JSON
    {

        public class RootobjectTopArtist
        {
            public string href { get; set; }
            public int limit { get; set; }
            public string next { get; set; }
            public int offset { get; set; }
            public object previous { get; set; }
            public int total { get; set; }
            public Top_Artist[] items { get; set; }
        }

        public class Top_Artist
        {
            public string[] genres { get; set; }
            public string id { get; set; }
            public Image[] images { get; set; }
            public string name { get; set; }
            public int popularity { get; set; }
        }

        public class Image
        {
            public string url { get; set; }
            public int height { get; set; }
            public int width { get; set; }
        }

    }
}
