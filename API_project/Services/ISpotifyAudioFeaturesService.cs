using API_project.Models;
using static API_project.Models.AudioFeatures;
using static API_project.Models.TopArtist;

namespace API_project.Services
{
    public interface ISpotifyAudioFeaturesService
    {
        Task<IEnumerable<Audio_Features>> GetRecentlyPLayedAudioFeatures(string trackIds, string token);
    }
}
