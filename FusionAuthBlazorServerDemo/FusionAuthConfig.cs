namespace FusionAuthBlazorServerDemo;

public record FusionAuthConfig
{
    public Uri Uri { get; init; } = new("http://localhost:9011");
    public string ClientId { get; init; } = "";
    public string ClientSecret { get; init; } = "";
    public string ApiKey { get; init; } = "";
    public bool? LogClaims { get; init; }
    
    public string TenantId { get; init; } = "";
}