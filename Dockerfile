# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copy the entire repo to the container
COPY . .

# Install .NET tools
RUN dotnet tool install --global dotnet-sonarscanner

# Make sure the path to tools is available
ENV PATH="${PATH}:/root/.dotnet/tools"

# Restore dependencies
RUN dotnet restore pbuild.sln  # Restore the solution from the root

# Build the application
RUN dotnet build pbuild.sln --configuration Release --no-restore

# Run tests and collect code coverage
RUN dotnet test pbuild-tests/pbuild-tests.csproj --no-build --configuration Release \
  --collect:"XPlat Code Coverage" --results-directory /app/TestResults

# Publish the app for production
RUN dotnet publish pbuild-api/pbuild-api.csproj --configuration Release --no-build --output /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

# Copy published files from the build stage
COPY --from=build /app/publish .

# Set the entrypoint to run the application
ENTRYPOINT ["dotnet", "pbuild-api.dll"]
