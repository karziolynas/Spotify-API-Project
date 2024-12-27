using API_project.Models;
using static API_project.Models.AudioFeatures;
using static API_project.Models.TopArtist;

namespace API_project.Services
{
    public interface ISpotifyTopArtistsService
    {
        Task<IEnumerable<TopArtist>> GetUsersTopArtists(string type, string timeRange, string limit, string offset, string token);
        List<string> setPopularity(IEnumerable<TopArtist> topArtistList);
        Dictionary<string, int> GetTopGenres(IEnumerable<TopArtist> topArtistList);
        string formQuery(IEnumerable<TopArtist> topArtists);
    }
}
