using API_project.Models;

namespace API_project.Services
{
    public interface IChartService
    {
        Task<string> GetChart(Dictionary<string, int> topGenres);
    }
}
