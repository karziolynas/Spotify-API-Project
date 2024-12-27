using API_project.Models;

namespace API_project.Services
{
    public interface ISpotifyService
    {
        Task<IEnumerable<Song>> GetRecentlyPlayed(int limit, string token);
    }
}
