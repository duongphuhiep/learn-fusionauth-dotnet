using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace FusionAuthBlazorServerDemo;

public class FusionAuthManager(IHttpContextAccessor httpContextAccessor)
{
    public const string FusionAuthScheme = "FusionAuth";
    
    public async Task SignOutAsync(string returnUrl = "/")
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext is null)
        {
            return;
        }

        await httpContext.SignOutAsync(
            IdentityConstants
                .ExternalScheme); // Options: signs you out entirely, without this you may not be reprompted for your password.

        await httpContext.SignOutAsync(
            FusionAuthScheme,
            new AuthenticationProperties { RedirectUri = returnUrl }
        );
    }
}