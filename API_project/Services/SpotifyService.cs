using API_project.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Xml.Linq;

namespace API_project.Services
{
    public class SpotifyService : ISpotifyService
    {
        private readonly HttpClient _httpClient;

        public SpotifyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Song>> GetRecentlyPlayed(int limit, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"me/player/recently-played?limit={limit}");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var responseObject = await JsonSerializer.DeserializeAsync<Rootobject>(responseStream);

            return responseObject?.items?.Select(i => new Song
            {
                albumName = i.track.album.name,
                name = i.track.name,
                id = i.track.id,
                artists = string.Join(",", i.track.artists.Select(i => i.name)),
                imageUrl = i.track.album.images.First().url
            });
        }
    }
}
