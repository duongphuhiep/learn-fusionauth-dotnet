# FusionAuth .NET Evaluation and Tutorial

This project is a part of my **evaluation of different self-hosted Authentication and Authorization as a Service (AAAS) solutions for a .NET Blazor application**. It also serves as a tutorial on how to **integrate [FusionAuth]** into a .NET Blazor Server application.

  * The [FusionAuth.Sdk.ConsoleApp](./FusionAuth.Sdk.ConsoleApp) demonstrates how to call the [FusionAuth]'s API.
  * The [FusionAuthBlazorServerDemo] demonstrates how to integrate [FusionAuth] into a Blazor Server application, including functionality for users to log in, log out, register a new account, unsubscribe from the application, and delete their account.

-----

## How to Run the Demo

### Setup FusionAuth

To get started, you'll need to set up FusionAuth.

1.  Start FusionAuth on your local machine by running Docker Compose in the [FusionAuthDevServer](./FusionAuthDevServer) folder.
2.  Once Docker Compose is up, you can access the FusionAuth admin console at `http://localhost:9011`.
3.  Configure a new Tenant, for example `Changebank`.
      * Set up the [SMTP settings](https://fusionauth.io/docs/customize/email-and-messages/configure-email) so FusionAuth can send emails via the local `mailcatcher`.
          * SMTP Host: `mailcatcher`
          * SMTP Port: `1025`
          * You can read emails from the host machine (outside the container) at `http://localhost:1080`.
      * Add a new Application and create roles such as `employee`, `manager`, and `consultant_ext`.
          * In the OAuth tab, configure the Redirect URLs to be `https://localhost:5301/signin-oidc` and `https://localhost:5301/signout-callback-oidc`.
4.  Put your configuration details to [appsettings.json](./FusionAuthBlazorServerDemo/appsettings.json).

**Note:** FusionAuth provides a convenient [Kickstart](https://fusionauth.io/docs/get-started/download-and-install/development/kickstart) feature, which auto-configure everything after the `docker-compose up` command. This project doesn't include a kickstart script, on purpose, so you'll configure FusionAuth manually to better understand the platform. Examples of kickstart scripts:

  * [Simple kickstart](https://github.com/FusionAuth/fusionauth-example-client-libraries/blob/main/kickstart/kickstart.json)
  * [Changebank's kickstart](https://github.com/FusionAuth/fusionauth-quickstart-dotnet-web/blob/main/kickstart/kickstart.json) with a branding theme.

### Run the Demo

Simply run or debug the [FusionAuthBlazorServerDemo] project.

You can then configure FusionAuth to add Passwordless or Passkey login with zero code changes. You don't need to [wait for .NET 10](https://www.youtube.com/live/YuuWHkQAsFY?t=1210s) for this functionality.

-----

## My Evaluation

### What I Like

  * **Maturity Product** 👴
    FusionAuth is a well-established product, used by many companies.

  * **Core Data Model** 🧩
    [The data models](https://fusionauth.io/docs/get-started/core-concepts/groups) (Tenants, Applications, Roles, Groups...) is simple, yet flexible and powerful.

  * **Customization Capabilities** 🎨
  	There are 2 integration methods: [Hosted login .vs Api login](https://fusionauth.io/docs/get-started/core-concepts/hosted-login-vs-api-login)
    I evaluated the recommended [Hosted login](https://fusionauth.io/docs/get-started/core-concepts/integration-points#hosted-login-pages).
	Unlike many AAAS solutions that limit branding and customization, FusionAuth allows [simple theming](https://fusionauth.io/docs/customize/look-and-feel/simple-theme-editor) or [extensive theming through FreeMarker](https://fusionauth.io/docs/customize/look-and-feel/advanced-theme-editor). You can even [develop custom themes locally with DaisyUI or TailwindCSS and upload them to your (self-hosted) FusionAuth via the API](https://fusionauth.io/docs/customize/look-and-feel/tailwind). While this requires some effort, the level of control is a significant advantage. [Email template customization](https://fusionauth.io/docs/customize/email-and-messages/configuring-application-specific-email-templates) and internationalization are also first-class features.

  * **The Kickstart Script** 🤖
    Kickstart is excellent for CI/CD pipelines, letting you consistently configure FusionAuth before integration tests.

  * **Documentation and the "ASK AI" Button** 📖
  	The documentation is clear and beginner-friendly, as shown in the [quickstart guide](https://fusionauth.io/docs/quickstarts/quickstart-dotnet-web).
	The "Ask AI" button is very useful.

  * **No "Contact Us" for Enterprise Plan** 💰

### What I Don't Like

  * **Current .NET SDK** 😔
    The current .NET SDK doesn't feel production-ready. See: [GitHub Issue](https://github.com/FusionAuth/fusionauth-netcore-client/issues/145).

  * **OpenAPI Specs** 📝
    The OpenAPI specification for the API is not exposed with the API itself; it's in a separate repository. This leads to the specifications not being in sync with the API, making them much less useful. This issue has been noted on GitHub: [FusionAuth OpenAPI Issue](https://github.com/FusionAuth/fusionauth-openapi/issues). The project's statement that they are "publishing this to see how useful the FusionAuth community finds it" suggests the OpenAPI spec is not a high priority.

  * **OpenSearch Dependency** 🕵️
    FusionAuth relies on OpenSearch. I'm skeptical because I'm not a fan of ElasticSearch-based tech.

  * **No free plan for cloud hosting** 💰
    If you want to "taste" FusionAuth on production then self hosting is the only option.

  * **Paid-Only Core Features** 💰
    Some features that should be default are paid-only. For example: `Unverified behavior: Gated`. By default, users can log in even if their registration isn't verified. Blocking this requires a paid plan.

	IMO if the registration is not verified, the user should not be able to log in by default. FusionAuth would suggest "registration auto-verification" so that the registration is verified automatically when it is created.

    My guess is this is due to backward compatibility for recently introduced features. Still, it means other important core features might also be paid-only. Carefully check the features you'll need.

## Conclusion

[FusionAuth] is a solid self-hosted AAAS solution. While .NET support is average, it's not a dealbreaker. Just ensure the features you need are included in the free self-hosted edition.

[FusionAuth]: https://fusionauth.io
[FusionAuthBlazorServerDemo]: ./FusionAuthBlazorServerDemo
