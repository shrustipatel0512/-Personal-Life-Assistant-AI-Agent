FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY NuGet.Config ./
COPY PersonalLifeAssistant.sln ./
COPY src/PersonalLifeAssistant.Api/PersonalLifeAssistant.Api.csproj src/PersonalLifeAssistant.Api/
COPY src/PersonalLifeAssistant.Application/PersonalLifeAssistant.Application.csproj src/PersonalLifeAssistant.Application/
COPY src/PersonalLifeAssistant.Domain/PersonalLifeAssistant.Domain.csproj src/PersonalLifeAssistant.Domain/
COPY src/PersonalLifeAssistant.Infrastructure/PersonalLifeAssistant.Infrastructure.csproj src/PersonalLifeAssistant.Infrastructure/

RUN dotnet restore src/PersonalLifeAssistant.Api/PersonalLifeAssistant.Api.csproj --configfile NuGet.Config

COPY . .
RUN dotnet publish src/PersonalLifeAssistant.Api/PersonalLifeAssistant.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

ENV PORT=8080 \
    Gemini__ApiKey= \
    Gemini__Model=gemini-2.5-flash \
    Gemini__BaseUrl=https://generativelanguage.googleapis.com/v1beta

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["sh", "-c", "ASPNETCORE_URLS=http://+:${PORT:-8080} dotnet PersonalLifeAssistant.Api.dll"]
