namespace Core.Network.Client;

public interface IApiTokenService
{
    Task<string> GetToken();

    Task RefreshAuthToken();
}