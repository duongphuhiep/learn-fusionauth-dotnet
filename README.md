# FusionAuth .NET Evaluation and Tutorial

This project is part of my evaluation on differents Self-host AAAS solution for a .NET Blazor applications.

You can also consider this repository as a tutorial showing how to integrate [FusionAuth] in a .NET Blazor Server application.

* The [FusionAuth.Sdk.ConsoleApp](./FusionAuth.Sdk.ConsoleApp) demonstrates how to call the [FusionAuth]'s API.
* The [FusionAuthBlazorServerDemo] demonstrates how to integrate [FusionAuth] in a Blazor Server application.
	* User can login/logout
	* User can register a new account
	* User can unsubscribe the application
	* User can remove their account

[FusionAuth]: https://fusionauth.io
[FusionAuthBlazorServerDemo]: ./FusionAuthBlazorServerDemo

## How to run the demo

### Setup FusionAuth

* Start FusionAuth on localhost by running Docker Compose in the folder [FusionAuthDevServer](./FusionAuthDevServer)
* Once the docker compose is Up, you can access the FusionAuth admin console at <http://localhost:9011>
* Configure a new Tenant (for eg: `Changebank`): 
	* Configure the [SMTP settings](https://fusionauth.io/docs/customize/email-and-messages/configure-email), so that FusionAuth can send email with the local `mailcatcher`.
		* SMTP Host: `mailcatcher`
		* SMTP Port: `1025`
		* On the host machine (outside the container), you can read emails at <http://localhost:1080>
	* Add a new Application, create Roles (`employee`, `manager`, `consultant_ext`) on this Application
		* Configure the Application (in the OAuth Tab) to use `https://localhost:5301/signin-oidc` and `https://localhost:5301/signout-callback-oidc` as Redirect URL
	
* Put your configuration in [appsettings.json](./FusionAuthBlazorServerDemo/appsettings.json)

Note: [FusionAuth] has a convenient feature called [Kickstart](https://fusionauth.io/docs/get-started/download-and-install/development/kickstart). 
A simple `docker-compose up` would also run the "kickstart" and setup everything. Unfortunately I didn't prepare the kickstart script here so you will have to 
manually configure FusionAuth. Here are some examples of the kickstart scripts: 

* [simple kickstart](https://github.com/FusionAuth/fusionauth-example-client-libraries/blob/main/kickstart/kickstart.json) 
* [changebank's kickstart](https://github.com/FusionAuth/fusionauth-quickstart-dotnet-web/blob/main/kickstart/kickstart.json) with branding theme.

However, in the context of the evaluation and Tutorial, it is better to configure FusionAuth manually to understand it better.

### Run the demo

Run / Debug the project [FusionAuthBlazorServerDemo]

## My evaluation

### I like the Kickstart script 

This "kickstart" functionnality is great for CI/CD to automatically and consistently configure the FusionAuth server before running integration tests.

### I like the Customization capabilities

There are 2 integration methods: [Hosted login .vs Api login](https://fusionauth.io/docs/get-started/core-concepts/hosted-login-vs-api-login)
I evaluated only the recommended method: [Hosted login](https://fusionauth.io/docs/get-started/core-concepts/integration-points#hosted-login-pages).

For other AAAS solution, this integration method usually has [limited capabilities on branding and customization](https://fusionauth.io/docs/customize/look-and-feel/simple-theme-editor). 
Fortunately, FusionAuth seem not have this limitation as it allow heavy [Theming with FreeMarker](https://fusionauth.io/docs/customize/look-and-feel/advanced-theme-editor). 

[We can even develop custom theme with DaisyUI or TailwindCSS on local then upload it to FusionAuth via API](https://fusionauth.io/docs/customize/look-and-feel/tailwind)

It requires some work, but at least it is possible.

The [Email template customization](https://fusionauth.io/docs/customize/email-and-messages/configuring-application-specific-email-templates), and internationalization are also first class citizens.

### I like the Core Concepts Data Model (Tenants, Applications, Roles, Groups..)

It feels flat / simple but still flexible and powerful
<https://fusionauth.io/docs/get-started/core-concepts/groups>

### I like the Documentation and the "ASK AI" button

[The doc is understandable for a AAAS novice like me](https://fusionauth.io/docs/quickstarts/quickstart-dotnet-web)

### I like the maturity of the product

FusionAuth is a mature product, it has been around for a while and it is used by many companies.

### I don't like the current .NET SDK

<https://github.com/FusionAuth/fusionauth-netcore-client/issues/145>

I don't think we should use it on production.

### I don't like the OpenAPI specs of the API

It should be exposed along with the API, not in a separate repository.
The API should expose an endpoint for eg "/openapi.yml" to expose the Specs.

Actually, as the OpenAPI specs is not part of the API, [it is not in sync with the API](https://github.com/FusionAuth/fusionauth-openapi/issues).

This OpenAPI specs is second class citizen as state: "We are publishing this to see how useful the FusionAuth community finds it."
=> As the OpenAPI specs is not guaranteed in sync with the API it is much less useful than it could be.

### I'm skeptical against the OpenSearch technology that FusionAuth is relying on

I'm not a fan of ElasticSearch

### An important core feature is paid only

For example: "Unverified behavior: Gated" 
By default user is allow to login the application evens if the registration is not verified. If you want to block that, then it is Paid :(

IMO if the registration is not verified, the user should not be able to login the application by default.
FusionAuth would suggest "registration auto-verification" so that the registration is verified automatically when it is created.

My guess: FusionAuth makes things this way maybe because it was a lately added functionality (for paid users) while they have to ensure backward compatibility.

There might be other important (core) functionalities that are paid only. I'm not dive in enough, you should check carefully the functionalities you might need.

## Conclusion

FusionAuth is a great Self host AAAS solution; but support for .NET is average (as long as the OpenAPI specs is still second class citizen or the .NET SDK is not usable on production).