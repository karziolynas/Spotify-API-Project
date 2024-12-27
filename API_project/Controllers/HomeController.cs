using API_project.Models;
using API_project.Services;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using static API_project.Models.AudioFeatures;
using static API_project.Models.TopArtist;

namespace API_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISpotifyServiceAccount _spotifyServiceAccount;
        private readonly IConfiguration _configuration;
        private readonly ISpotifyService _spotifyService;
        //private readonly ISpotifyAudioFeaturesService _spotifyServiceAudioFeatures;
        private readonly ISpotifyTopArtistsService _spotifyTopArtistsService;
        private readonly IChartService _chartService;
        private readonly ITasteDiveService _tasteDiveService;
        private readonly string redirectUri = "https://localhost:7075/Home/Callback";
        private string token;

        public HomeController(ISpotifyServiceAccount spotifyServiceAccount, IConfiguration configuration,
            ISpotifyService spotifyService, //ISpotifyAudioFeaturesService spotifyServiceAudioFeatures,
            ISpotifyTopArtistsService spotifyTopArtistsService, IChartService chartService,
            ITasteDiveService tasteDiveService)
        {
            _spotifyServiceAccount = spotifyServiceAccount;
            _configuration = configuration;
            _spotifyService = spotifyService;
            //_spotifyServiceAudioFeatures = spotifyServiceAudioFeatures;
            _spotifyTopArtistsService = spotifyTopArtistsService;
            _chartService = chartService;
            _tasteDiveService = tasteDiveService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Callback()
        {
            //var recentlyPlayed = await getRecentlyPlayed();

            var topArtists = await getTopArtists();
            ViewBag.Popularity = _spotifyTopArtistsService.setPopularity(topArtists);
            Dictionary<string, int> topGenres;
            topGenres = _spotifyTopArtistsService.GetTopGenres(topArtists);
            ViewData["ChartPath"] = await _chartService.GetChart(topGenres);

            //string trackIds = string.Join(",", recentlyPlayed.Select(item => item.id));
            //var audioFeatures = await getRecentlyPlayedAudioFeatures(trackIds, token);

            string query = _spotifyTopArtistsService.formQuery(topArtists);
            ViewBag.Reccomendations = await _tasteDiveService.GetReccommendations(query, "10", _configuration["TasteDive:ApiKey"], "1");

            return View(topArtists);
        }

        private async Task<IEnumerable<Song>> getRecentlyPlayed()
        {
            // get the code and the state from the URL
            // after the request for auth token
            string code = HttpContext.Request.Query["code"];
            string state = HttpContext.Request.Query["state"];

            try
            {
                 token = await _spotifyServiceAccount.GetToken(_configuration["Spotify:ClientId"],
                _configuration["Spotify:ClientSecret"], code, redirectUri);

                var recentSongs = await _spotifyService.GetRecentlyPlayed(15, token);
                return recentSongs;
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
                return Enumerable.Empty<Song>();
            }
        }

        private async Task<IEnumerable<TopArtist>> getTopArtists()
        {
            // get the code and the state from the URL
            // after the request for auth token
            string code = HttpContext.Request.Query["code"];
            string state = HttpContext.Request.Query["state"];

            try
            {
                token = await _spotifyServiceAccount.GetToken(_configuration["Spotify:ClientId"],
               _configuration["Spotify:ClientSecret"], code, redirectUri);

                var topArtists = await _spotifyTopArtistsService.GetUsersTopArtists("artists", "short_term", "15","0", token);
                return topArtists;
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
                return Enumerable.Empty<TopArtist>();
            }
        }

        //private async Task<IEnumerable<Audio_Features>> getRecentlyPlayedAudioFeatures(string trackIds, string token)
        //{
        //    try
        //    {
        //        var audioFeatures = await _spotifyServiceAudioFeatures.GetRecentlyPLayedAudioFeatures(trackIds, token);
        //        return audioFeatures;
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.Write(ex);
        //        return Enumerable.Empty<Audio_Features>();
        //    }
        //}

        [HttpPost]
        public IActionResult OnButtonClick()
        {
            string clientID = _configuration["Spotify:ClientId"];
            string responseType = "code";

            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuwxyz0123456789";
            string state = new string(Enumerable.Repeat(chars, 12).Select(s => s[random.Next(s.Length)]).ToArray());

            string scope = "user-read-private user-read-recently-played user-top-read";

            string uri = $"https://accounts.spotify.com/authorize?client_id={clientID}" +
                $"&response_type={responseType}&redirect_uri={redirectUri}" +
                $"&state={state}&scope={scope}";

            return Redirect(uri);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
