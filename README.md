# FusionAuth .NET Evaluation and Tutorial

This project is a part of my **evaluation of different self-hosted Authentication and Authorization as a Service (AAAS) solutions for a .NET Blazor application**. It also serves as a tutorial on how to **integrate [FusionAuth]** into a .NET Blazor Server application.

  * The [FusionAuth.Sdk.ConsoleApp](./FusionAuth.Sdk.ConsoleApp) demonstrates how to call the [FusionAuth]'s API.
  * The [FusionAuthBlazorServerDemo] demonstrates how to integrate [FusionAuth] into a Blazor Server application, including functionality for users to log in, log out, register a new account, unsubscribe from the application, and remove their account.

-----

## How to Run the Demo

### Setup FusionAuth

To get started, you'll need to set up FusionAuth.

1.  Start FusionAuth on your local machine by running Docker Compose in the [FusionAuthDevServer](./FusionAuthDevServer) folder.
2.  Once Docker Compose is up, you can access the FusionAuth admin console at `http://localhost:9011`.
3.  Configure a new Tenant, such as `Changebank`.
      * Set up the [SMTP settings](https://fusionauth.io/docs/customize/email-and-messages/configure-email) so FusionAuth can send emails with the local `mailcatcher`.
          * SMTP Host: `mailcatcher`
          * SMTP Port: `1025`
          * You can read emails from the host machine (outside the container) at `http://localhost:1080`.
      * Add a new Application and create roles for it, such as `employee`, `manager`, and `consultant_ext`.
          * In the OAuth tab of the Application, configure the Redirect URLs to be `https://localhost:5301/signin-oidc` and `https://localhost:5301/signout-callback-oidc`.
4.  Put your configuration details in the [appsettings.json](./FusionAuthBlazorServerDemo/appsettings.json).

**Note:** FusionAuth has a convenient feature called [Kickstart](https://fusionauth.io/docs/get-started/download-and-install/development/kickstart), which allows for automatic configuration with a simple `docker-compose up` command. Unfortunately, this project doesn't include a kickstart script, so you'll need to configure FusionAuth manually. This manual process is intentional, as it helps with understanding the platform better during the evaluation and tutorial. Examples of kickstart scripts can be found here:

  * [Simple kickstart](https://github.com/FusionAuth/fusionauth-example-client-libraries/blob/main/kickstart/kickstart.json)
  * [Changebank's kickstart](https://github.com/FusionAuth/fusionauth-quickstart-dotnet-web/blob/main/kickstart/kickstart.json) with a branding theme.

### Run the Demo

Simply run or debug the [FusionAuthBlazorServerDemo] project.

-----

## My Evaluation

### What I Like

  * **Maturity of the Product** 👴
    FusionAuth is a mature product that has been around for a while and is used by many companies.

  * **Core Concepts Data Model** 🧩
    [The data models](https://fusionauth.io/docs/get-started/core-concepts/groups) with concepts like **Tenants**, **Applications**, **Roles**, and **Groups**—feels simple, yet it's flexible and powerful.

  * **Customization Capabilities** 🎨
  	There are 2 integration methods: [Hosted login .vs Api login](https://fusionauth.io/docs/get-started/core-concepts/hosted-login-vs-api-login)
    I evaluated the recommended integration method: [Hosted login](https://fusionauth.io/docs/get-started/core-concepts/integration-points#hosted-login-pages). 
	For other AAAS solutions, this integration method usually has [limited capabilities on branding and customization](https://fusionauth.io/docs/customize/look-and-feel/simple-theme-editor). But unlike them, FusionAuth offers extensive customization through [Theming with FreeMarker](https://fusionauth.io/docs/customize/look-and-feel/advanced-theme-editor). You can even [develop custom themes locally with DaisyUI or TailwindCSS and upload them to your (self-hosted) FusionAuth via the API](https://fusionauth.io/docs/customize/look-and-feel/tailwind). While this requires some effort, the level of control is a significant advantage. [Email template customization](https://fusionauth.io/docs/customize/email-and-messages/configuring-application-specific-email-templates) and internationalization are also first-class features.	
  
  * **The Kickstart Script** 🤖
    This "kickstart" functionality is great for CI/CD pipelines, as it allows you to automatically and consistently configure the FusionAuth server before running integration tests.
	
  * **Documentation and the "ASK AI" Button** 📖
    The documentation is well-written and easy for a novice to understand, as seen in the [quickstart guide](https://fusionauth.io/docs/quickstarts/quickstart-dotnet-web). The "Ask AI" button is very helpful.
  
### What I Don't Like

  * **Current .NET SDK** 😔
    I don't believe the current .NET SDK is ready for production use. You can read more about the issues here: [FusionAuth GitHub Issue \#145](https://github.com/FusionAuth/fusionauth-netcore-client/issues/145).
	
  * **OpenAPI Specs** 📝
    The OpenAPI specification for the API is not exposed with the API itself; it's in a separate repository. This leads to the specifications not being in sync with the API, making them much less useful. This issue has been noted on GitHub: [FusionAuth OpenAPI Issue](https://github.com/FusionAuth/fusionauth-openapi/issues). The project's statement that they are "publishing this to see how useful the FusionAuth community finds it" suggests the OpenAPI spec is not a high priority.
	
  * **OpenSearch Technology** 🕵️
    I'm skeptical of FusionAuth's reliance on OpenSearch, as I'm not a fan of ElasticSearch.
	
  * **Paid-Only Core Features** 💰
    For example, "Unverified behavior: Gated" is a paid feature.
	By default, users are allowed to log into the application even if their registration is not verified. If you want to block this behavior, it's a paid feature. 
	
	IMO if the registration is not verified, the user should not be able to log in by default. FusionAuth would suggest "registration auto-verification" so that the registration is verified automatically when it is created.
	
    My guess is that FusionAuth made it this way because it was a recently added functionality (for paid users) and they had to ensure backward compatibility. There might be other important (core) functionalities that are paid only. You should carefully check the functionalities you might need.

## Conclusion

[FusionAuth] is a great self-hosted AAAS solution. Although its support for .NET is average, it is not a blocker. However, you should make sure that the features you need are available in the free Self-hosted edition.

[FusionAuth]: https://fusionauth.io
[FusionAuthBlazorServerDemo]: ./FusionAuthBlazorServerDemo

