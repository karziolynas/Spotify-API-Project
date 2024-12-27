using API_project.Models;
using System;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Xml.Linq;

using static API_project.Models.TopArtist;
using static API_project.Models.TopArtist_JSON;

namespace API_project.Services
{
    public class SpotifyTopArtistsService : ISpotifyTopArtistsService
    {
        private readonly HttpClient _httpClient;

        public SpotifyTopArtistsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<TopArtist>> GetUsersTopArtists(string type, string timeRange, string limit, string offset, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"top/artists?time_range={timeRange}&limit={limit}&offset={offset}");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var responseObject = await JsonSerializer.DeserializeAsync<RootobjectTopArtist>(responseStream);

            return responseObject?.items?.Select(i => new TopArtist
            {
                genres = String.Join(",", i.genres),
                name = i.name,
                id = i.id,
                imageUrl = i.images.First().url,
                popularity = i.popularity,
                popularityText = ""
            });
        }

        public List<string> setPopularity(IEnumerable<TopArtist> topArtistList)
        {
            List<string> popularityList = new List<string>();
            for(int i = 0; i < topArtistList.Count(); i++)
            {
                int popularity = topArtistList.ElementAt(i).popularity;
                if (popularity < 25)
                {
                    popularityList.Add("Not very popular");
                }
                else if (popularity >= 25 && popularity < 50)
                {
                    popularityList.Add("A little popular");
                }
                else if (popularity >= 50 && popularity < 75)
                {
                    popularityList.Add("Popular");
                }
                else if (popularity >= 75)
                {
                    popularityList.Add("Very popular");
                }
            }
            return popularityList;
        }

        public Dictionary<string, int> GetTopGenres(IEnumerable<TopArtist> topArtistList)
        {
            Dictionary<string, int> topGenres = new Dictionary<string, int>();
            topGenres.Add("metal", 0);
            topGenres.Add("rock", 0);
            topGenres.Add("pop", 0);
            topGenres.Add("hip hop", 0);
            topGenres.Add("rap", 0);
            topGenres.Add("r&b", 0);
            topGenres.Add("soul", 0);
            topGenres.Add("country", 0);
            topGenres.Add("jazz", 0);
            topGenres.Add("folk", 0);
            topGenres.Add("punk", 0);
            topGenres.Add("funk", 0);
            topGenres.Add("indie", 0);

            foreach (var topArtist in topArtistList)
            {
                
                if (topArtist.genres.Contains("metal"))
                {
                    topGenres["metal"] += 1;
                }
                if (topArtist.genres.Contains("rock"))
                {
                    topGenres["rock"] += 1;
                }
                if (topArtist.genres.Contains("pop"))
                {
                    topGenres["pop"] += 1;
                }
                if (topArtist.genres.Contains("hip hop"))
                {
                    topGenres["hip hop"] += 1;
                }
                if (topArtist.genres.Contains("rap"))
                {
                    topGenres["rap"] += 1;
                }
                if (topArtist.genres.Contains("r&b"))
                {
                    topGenres["r&b"] += 1;
                }
                if (topArtist.genres.Contains("soul"))
                {
                    topGenres["soul"] += 1;
                }
                if (topArtist.genres.Contains("country"))
                {
                    topGenres["country"] += 1;
                }
                if (topArtist.genres.Contains("jazz"))
                {
                    topGenres["jazz"] += 1;
                }
                if (topArtist.genres.Contains("folk"))
                {
                    topGenres["folk"] += 1;
                }
                if (topArtist.genres.Contains("punk"))
                {
                    topGenres["punk"] += 1;
                }
                if (topArtist.genres.Contains("funk"))
                {
                    topGenres["funk"] += 1;
                }
                if (topArtist.genres.Contains("indie"))
                {
                    topGenres["indie"] += 1;
                }

            }
            return topGenres;

        }

        public string formQuery(IEnumerable<TopArtist> topArtists)
        {
            string query = "";
            for (int i = 3; i < 8; i++)
            {
                if (i == 7)
                {
                    query = query + "music:" + topArtists.ElementAt(i).name.ToLower();
                    break;
                }
                query = query + "music:" + topArtists.ElementAt(i).name.ToLower() + ",";
            }

            return query;
        }
    }
}
