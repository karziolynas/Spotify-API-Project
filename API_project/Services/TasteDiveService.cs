using API_project.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using static API_project.Models.Recommendations;

namespace API_project.Services
{
    public class TasteDiveService : ITasteDiveService
    {
        private readonly HttpClient _httpClient;

        public TasteDiveService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Result>> GetReccommendations(string query, string limit, string key, string slimit)
        {
            query = Uri.EscapeDataString(query);
            string parameters = $"{query}&type=music&limit={limit}&k={key}&slimit={slimit}";
            var response = await _httpClient.GetAsync($"https://tastedive.com/api/similar?q={parameters}");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var responseObject = await JsonSerializer.DeserializeAsync<RootobjectReco>(responseStream);

            return responseObject?.similar.results?.Select(i => new Result
            {
                name = i.name,
                type = i.type
            });
        }
    }
}
