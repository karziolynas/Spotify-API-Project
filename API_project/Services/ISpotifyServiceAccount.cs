namespace API_project.Services
{
    public interface ISpotifyServiceAccount
    {
        Task<string> GetToken(string clientId, string clientSecret, string code, string redirectUri);
    }
}
