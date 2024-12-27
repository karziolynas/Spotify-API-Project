namespace API_project.Models
{
    public class AudioFeatures
    {

        public class RootobjectAudioFeatures
        {
            public Audio_Features[] audio_features { get; set; }
        }

        public class Audio_Features
        {
            public float acousticness { get; set; }
            public string analysis_url { get; set; }
            public float danceability { get; set; }
            public int duration_ms { get; set; }
            public float energy { get; set; }
            public string id { get; set; }
            public float instrumentalness { get; set; }
            public int key { get; set; }
            public float liveness { get; set; }
            public float loudness { get; set; }
            public int mode { get; set; }
            public float speechiness { get; set; }
            public float tempo { get; set; }
            public int time_signature { get; set; }
            public string track_href { get; set; }
            public string type { get; set; }
            public string uri { get; set; }
            public float valence { get; set; }
        }

    }
}
