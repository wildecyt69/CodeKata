namespace Core.Network.Client;

public class NetworkServiceOptions
{
    public Dictionary<string, string> ApiUrls { get; set; }
    public string AppName { get; set; }
    public string Audience { get; set; }
    public string BaseAPIUrl { get; set; }
    public string IDPUrl { get; set; }
    public string Issuer { get; set; }
    public string JwtSecret { get; set; }
    public Guid MachineGuid { get; set; }
    public IList<string> Scopes { get; set; }
    public int TokenExpirationHours { get; set; }
}