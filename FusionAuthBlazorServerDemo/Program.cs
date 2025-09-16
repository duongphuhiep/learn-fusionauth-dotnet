using FusionAuth.Sdk;
using FusionAuthBlazorServerDemo;
using FusionAuthBlazorServerDemo.Components;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using ToolsPack.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

#region Register FusionAuth Auhentication

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<FusionAuthManager>();
builder.Services.AddSingleton<IConfigureOptions<OpenIdConnectOptions>, FusionAuthOpenIdConnectOptions>()
    .Configure<FusionAuthConfig>(
        builder.Configuration.GetSection("FusionAuth")
    );
builder.Services
    .AddAuthorization()
    .AddAuthentication(FusionAuthManager.FusionAuthScheme)
    .AddOpenIdConnect(FusionAuthManager.FusionAuthScheme, _ =>
    {
        // FusionAuthOpenIdConnectOptions (which we registered above) did the necessary configuration of the OpenIdConnect
        // You can override certain configuration here if needed
    })
    .AddExternalCookie()
    .Configure(o =>
    {
        o.Cookie.HttpOnly = true;
        o.Cookie.IsEssential = true;
        o.Cookie.SameSite = SameSiteMode.None;
        o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

#endregion

#region Register FusionAuthClient

builder.Services.AddKiotaHandlers()
    .AddTransient<CompactHttpLoggingMiddleware>();
// We don't use ApiKeyAuthenticationProvider because it requires HTTPS.
// We use AnonymousAuthenticationProvider then set the ApiKey to the "Authorization" header of the HttpClient later
// in the HttpClientWithApiKeyRequestAdapter.
// Another choice is to directly use the HttpClientRequestAdapter and configure the "Authorization" header here, and so
// the HttpClientWithApiKeyRequestAdapter would become useless and can be removed.
builder.Services.AddSingleton<IAuthenticationProvider, AnonymousAuthenticationProvider>();
builder.Services.AddHttpClient<HttpClientWithApiKeyRequestAdapter>()
    //The CompactHttpLoggingMiddleware logs all the Http request/response to the FusionAuth's API, it is better to attach
    //it before other Kiota handlers So that it will log the final requests after all the kiota's transformations
    //(if there is any)
    .AddHttpMessageHandler<CompactHttpLoggingMiddleware>()
    .AttachKiotaHandlers();
builder.Services.AddTransient<IRequestAdapter>(sp => sp.GetRequiredService<HttpClientWithApiKeyRequestAdapter>());
builder.Services.AddTransient<FusionAuthClient>();

#endregion

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapStaticAssets().AllowAnonymous();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapPost("Account/Logout", async (
    [FromServices] FusionAuthManager fusionAuthManager,
    [FromForm] string returnUrl) =>
{
    await fusionAuthManager.SignOutAsync();
});

app.Run();