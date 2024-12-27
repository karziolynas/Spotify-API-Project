namespace API_project.Models
{
    public class Recommendations
    {

        public class RootobjectReco
        {
            public Similar similar { get; set; }
        }

        public class Similar
        {
            public Info[] info { get; set; }
            public Result[] results { get; set; }
        }

        public class Info
        {
            public string name { get; set; }
            public string type { get; set; }
        }

        public class Result
        {
            public string name { get; set; }
            public string type { get; set; }
        }

    }
}
