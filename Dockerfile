# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app
COPY . . 

# Install Node.js and coveralls-lcov
RUN apt-get update && apt-get install -y nodejs npm
RUN npm install -g coveralls-lcov

# Restore dependencies
RUN dotnet restore
RUN dotnet build --configuration Release --no-restore
RUN dotnet test --no-build --configuration Release --collect:"XPlat Code Coverage"

# Convert Cobertura to LCOV
RUN coveralls-lcov /app/TestResults/*/coverage.cobertura.xml > /app/TestResults/coverage.lcov

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app
COPY --from=build /app/pbuild-api/bin/Release/net8.0/ .
ENTRYPOINT ["dotnet", "pbuild-api.dll"]
