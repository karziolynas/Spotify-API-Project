using API_project.Models;
using Humanizer.Localisation.TimeToClockNotation;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace API_project.Services
{
    public class ChartService : IChartService
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ChartService(HttpClient httpClient, IWebHostEnvironment webHostEnvironment)
        {
            _httpClient = httpClient;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> GetChart(Dictionary<string, int> topGenres)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "chart");

            int count = topGenres.Values.Count(value => value > 0);
            string[] genres = new string[count];
            int[] listenCount = new int[count];
            int j = 0;

            foreach(var genre in topGenres)
            {
                if (genre.Value > 0) // only shows the genres that are listened to
                {
                    genres[j] = genre.Key;
                    listenCount[j] = genre.Value;
                    j++;
                }
            }

            var chartConfig = new
            {
                type = "bar", 
                data = new
                {
                    labels = genres,
                    datasets = new[]
                    {
                        new
                        {
                            label = "Artists",
                            data = listenCount,
                            backgroundColor = new[] { "rgba(29, 185, 84, 1)" },
                            borderColor = new[] { "rgba(0, 0, 0, 1)" },
                            borderWidth = 1
                        }
                    }
                }
            };

            string jsonPayload = JsonSerializer.Serialize(new { chart = JsonSerializer.Serialize(chartConfig) });
            request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();

            // Save the image to a temporary file
            string imageFileName = $"chart_{Guid.NewGuid()}.png";
            string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", imageFileName);

            Directory.CreateDirectory(Path.GetDirectoryName(imagePath)); // Ensure directory exists
            await System.IO.File.WriteAllBytesAsync(imagePath, imageBytes);

            // Return the relative path for use in the view
            return $"/images/{imageFileName}";

        }
    }
}
