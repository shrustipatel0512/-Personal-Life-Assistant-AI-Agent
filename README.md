# Personal Life Assistant AI

Production-oriented starter blueprint for a Personal Life Assistant AI Agent using:

- Backend: ASP.NET Core Web API (.NET 8)
- Frontend: Angular standalone app structure
- Database: PostgreSQL
- AI provider: Google Gemini API

## Solution Layout

- `src/PersonalLifeAssistant.Domain`: entities and enums
- `src/PersonalLifeAssistant.Application`: CQRS, MediatR handlers, AI contracts
- `src/PersonalLifeAssistant.Infrastructure`: EF Core, PostgreSQL, Gemini, JWT wiring
- `src/PersonalLifeAssistant.Api`: REST API controllers and startup
- `frontend`: Angular starter app structure
- `docs`: architecture, schema, endpoints, prompts, scalability notes

## Quick Start

1. Set your Gemini API key in `src/PersonalLifeAssistant.Api/appsettings.json` or user secrets.
2. Restore packages:
   - `dotnet restore`
3. Run the API:
   - `dotnet run --project src/PersonalLifeAssistant.Api`
4. Review the Angular app in `frontend` and run:
   - `npm install`
   - `npm start`

## Notes

- The current environment blocked package restore, so this starter is scaffolded but not fully restored here.
- Use EF Core migrations after restore:
  - `dotnet ef migrations add InitialCreate --project src/PersonalLifeAssistant.Infrastructure --startup-project src/PersonalLifeAssistant.Api`
  - `dotnet ef database update --project src/PersonalLifeAssistant.Infrastructure --startup-project src/PersonalLifeAssistant.Api`
