using API_project.Models;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Xml.Linq;
using static API_project.Models.AudioFeatures;

namespace API_project.Services
{
    public class SpotifyAudioFeaturesService : ISpotifyAudioFeaturesService
    {
        private readonly HttpClient _httpClient;

        public SpotifyAudioFeaturesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Audio_Features>> GetRecentlyPLayedAudioFeatures(string trackIds, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"audio-features?ids={trackIds}");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var responseObject = await JsonSerializer.DeserializeAsync<RootobjectAudioFeatures>(responseStream);

            return responseObject?.audio_features?.Select(i => new Audio_Features
            {
                acousticness = i.acousticness,
                danceability = i.danceability,
                energy = i.energy,
                liveness = i.liveness,
                loudness = i.loudness,
                tempo = i.tempo,
                valence = i.valence
            });
        }
    }
}
