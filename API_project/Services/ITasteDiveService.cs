using API_project.Models;
using static API_project.Models.Recommendations;

namespace API_project.Services
{
    public interface ITasteDiveService
    {
        Task<IEnumerable<Result>> GetReccommendations(string query, string limit, string key, string slimit);
    }
}
