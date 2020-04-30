# AppTemplate - Template for a new .NET Core 3.1 application

Use this repository as a solution template to create a new application. Clone this repository, and do a find and replace in the solution of `MyApp` with whatever name you'd like for your application.

Comes pre-configured with a core and data layer, web api, unit test and integration test projects. Also includes a sample service to showcase how to properly implement a new service and expose it via the web api.

## Architecture

- MyApp.Web.Api - Web API 
- MyApp.Core - Houses core functionality like services, configuration, entity models, mappers
- MyApp.Data - Houses data specifically required to connect the entities defined in Core into and out of a database via the DataAccess implementation
- MyApp.Contracts - Houses contracts for use between different applications that rely on MyApp views. Should be built as a nuget package.
- Test
  - MyApp.Core.Test - Sample unit test project for the Core project. Any additional projects added should have their own .Test projects defined.
  - MyApp.Web.Api.Test - Integration test project for the Web API. Includes sample POST and PUT unit tests using an in-memory EF database
