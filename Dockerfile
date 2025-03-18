# Use .NET SDK for building and testing
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copy everything and restore dependencies
COPY *.sln .  
COPY pbuild-api/ pbuild-api/
COPY pbuild-business/ pbuild-business/
COPY pbuild-data/ pbuild-data/
COPY pbuild-domain/ pbuild-domain/
COPY pbuild-tests/ pbuild-tests/

RUN dotnet restore

# Build the application
RUN dotnet build --configuration Release --no-restore

# Run tests
RUN dotnet test --no-build --configuration Release

# Use the .NET runtime for the final application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

# Copy built files from the build stage
COPY --from=build /app/pbuild-api/bin/Release/net8.0/ .

ENTRYPOINT ["dotnet", "pbuild-api.dll"]
