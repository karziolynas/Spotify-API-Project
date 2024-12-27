
using API_project.Models;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.AccessControl;
using System.Text;
using System.Text.Json;
using static System.Formats.Asn1.AsnWriter;

namespace API_project.Services
{
    public class SpotifyServiceAccount : ISpotifyServiceAccount
    {
        private readonly HttpClient _httpClient;

        public SpotifyServiceAccount(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetToken(string clientId, string clientSecret, string code, string redirectUri)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "token");

            // Create the form data content first
            var formData = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"grant_type", "authorization_code"},
                {"code", code},
                {"redirect_uri", redirectUri}
            });

            // Set Content-Type header for the form data
            formData.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            // Assign the form data to the request content
            request.Content = formData;

            // Set the Authorization header for Basic authentication
            request.Headers.Authorization = new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"))
            );

            // Send the request and ensure a successful response
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // Read and deserialize the response content
            using var responseStream = await response.Content.ReadAsStreamAsync();
            var authResult = await JsonSerializer.DeserializeAsync<Auth>(responseStream);
            
            // Return the access token
            return authResult.access_token;
        }


    }
}
