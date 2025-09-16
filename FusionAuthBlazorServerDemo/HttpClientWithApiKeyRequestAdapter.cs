using Microsoft.Extensions.Options;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace FusionAuthBlazorServerDemo;

public class HttpClientWithApiKeyRequestAdapter : HttpClientRequestAdapter
{
    public HttpClientWithApiKeyRequestAdapter(
        IAuthenticationProvider authenticationProvider,
        HttpClient httpClient,
        IOptions<FusionAuthConfig> fusionAuthConfigOption,
        IParseNodeFactory? parseNodeFactory = null, 
        ISerializationWriterFactory? serializationWriterFactory = null,
        ObservabilityOptions? observabilityOptions = null) 
        : base(authenticationProvider, parseNodeFactory, serializationWriterFactory, httpClient, observabilityOptions)
    {
        httpClient.BaseAddress = fusionAuthConfigOption.Value.Uri;
        httpClient.DefaultRequestHeaders.Add("Authorization", fusionAuthConfigOption.Value.ApiKey);
    }
}