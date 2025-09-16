using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace FusionAuthBlazorServerDemo;

public class FusionAuthOpenIdConnectOptions(
    IOptions<FusionAuthConfig> fusionAuthConfigOption,
    IWebHostEnvironment webHostEnvironment,
    ILogger<FusionAuthOpenIdConnectOptions> logger) : IConfigureNamedOptions<OpenIdConnectOptions>
{
    public void Configure(OpenIdConnectOptions options)
    {
        var fusionAuthConfig = fusionAuthConfigOption.Value;
        options.ClientId = fusionAuthConfig.ClientId;
        options.Authority = fusionAuthConfig.Uri.ToString();
        options.ClientSecret = fusionAuthConfig.ClientSecret;
        options.SignInScheme = IdentityConstants.ExternalScheme;
        options.RequireHttpsMetadata = !webHostEnvironment.IsDevelopment();
        options.SaveTokens = true;
        options.ResponseType = OpenIdConnectResponseType.Code;
        options.UsePkce = true;
        options.GetClaimsFromUserInfoEndpoint = true;

        foreach (var scope in new[] { "email", "address", "phone" })
        {
            options.Scope.Add(scope);
        }

        options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
        options.ClaimActions.MapJsonKey(ClaimTypes.Locality, "locale");

        if (fusionAuthConfig.LogClaims != true) return;
        
        if (options.Events?.OnTokenValidated is not null)
        {
            logger.LogWarning($"You configured FusionAuth to log original Claims when user login ({nameof(FusionAuthConfig.LogClaims)} = true). This config is skipped, the origin claims won't be logged because the {nameof(OpenIdConnectEvents.OnTokenValidated)} configuration has been set (somewhere else), and We won't override it.");
            return;
        }
        options.Events ??= new OpenIdConnectEvents();
        options.Events.OnTokenValidated = LogClaims;
    }

    private Task LogClaims(TokenValidatedContext context)
    {
        IEnumerable<Claim>? claims = context.Principal?.Claims;
        if (claims != null)
        {
            var claimData = claims
                .GroupBy(c => c.Type)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(c => c.Value).ToArray()
                );
            logger.LogInformation("Claims from FusionAuth: {Claims}", JsonSerializer.Serialize(claimData));
        }
        else
        {
            logger.LogInformation("No claims found from FusionAuth");
        }

        return Task.CompletedTask;
    }
    
    public void Configure(string? name, OpenIdConnectOptions options)
    {
        //if (name != FusionAuthScheme) return;
        Configure(options);
    }
}