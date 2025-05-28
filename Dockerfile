FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copy only the project files first (better caching)
COPY *.sln ./
COPY pbuild-api/*.csproj ./pbuild-api/
COPY pbuild-business/*.csproj ./pbuild-business/
COPY pbuild-data/*.csproj ./pbuild-data/
COPY pbuild-domain/*.csproj ./pbuild-domain/

# Restore dependencies
RUN dotnet restore

# Copy the rest of the source code
COPY . .

# Build and publish
RUN dotnet publish pbuild-api/pbuild-api.csproj \
    --configuration Release \
    --no-restore \
    --output /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

# Configure ASP.NET Core
ENV ASPNETCORE_URLS=http://+:5286
ENV ASPNETCORE_ENVIRONMENT=Production

# Add Watchtower labels
LABEL com.centurylinklabs.watchtower.enable=true

EXPOSE 5286

ENTRYPOINT ["dotnet", "pbuild-api.dll"]