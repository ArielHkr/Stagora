# Image Docker de l'API StagoraAPI (utilisée par Render pour le déploiement).

# --- Étape 1 : compiler avec le SDK .NET ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Restaurer d'abord les paquets : Docker garde cette étape en cache
# tant que le .csproj ne change pas, ce qui accélère les builds suivants.
COPY StagoraAPI.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app/publish --no-restore

# --- Étape 2 : exécuter avec l'image ASP.NET, plus légère (sans le SDK) ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

# Ne pas exécuter l'application en tant qu'administrateur (root).
USER $APP_UID

ENTRYPOINT ["dotnet", "StagoraAPI.dll"]
