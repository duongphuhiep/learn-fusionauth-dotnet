using FusionAuth.Sdk;
using FusionAuth.Sdk.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Microsoft.Kiota.Serialization;
using ToolsPack.Logging;

try
{
    ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
    
    var handlers = KiotaClientFactory.CreateDefaultHandlers();
    handlers.Add(new CompactHttpLoggingMiddleware(loggerFactory.CreateLogger<CompactHttpLoggingMiddleware>()));
    var httpMessageHandler =
        KiotaClientFactory.ChainHandlersCollectionAndGetFirstLink(
            KiotaClientFactory.GetDefaultHttpMessageHandler(),
            handlers.ToArray());
    
    var httpClient = new HttpClient(httpMessageHandler!)
    {
        BaseAddress = new Uri("http://localhost:9011/"),
    };

    // Add the API key to default headers
    httpClient.DefaultRequestHeaders.Add("Authorization", "yYsyqDt6dLrAUZxJkFXWuLHvBc5YVI2SfclBrxVx-e0-RxREtWS30Sn0");

    IRequestAdapter requestAdapter = new HttpClientRequestAdapter(
        authenticationProvider: new AnonymousAuthenticationProvider(),
        httpClient: httpClient
    );

    var fusionAuthClient = new FusionAuthClient(requestAdapter);

    var userResponse = await fusionAuthClient.Api.User["24004c7b-137b-4bc1-baed-776a5740f406"].GetAsync();
    if (userResponse is null)
    {
        Console.WriteLine("User not found");
        return;
    }

    Console.WriteLine(await userResponse.SerializeAsJsonStringAsync());

    RegistrationResponse? registrationResponse = await fusionAuthClient.Api.User.Registration.PostAsync(
        new RegistrationRequest()
        {
            User = new User()
            {
                Email = "totochan@email.com",
                FullName = "Toto Chan",
                Password = "Lemon.1234",
                Username = "totochan",
            },
            Registration = new UserRegistration()
            {
                ApplicationId = new Guid("5084fd6c-d334-4565-9ba6-a67b767c4eb6"),
                Roles = new UntypedArray([new UntypedString("employee")])
            },
        });
    Console.WriteLine(await registrationResponse.SerializeAsJsonStringAsync());

}
catch (FusionAuth.Sdk.Models.Errors ex)
{
    Console.Error.WriteLine(ex.Message); 
}
catch (Exception e)
{
    Console.Error.WriteLine(e);
}

