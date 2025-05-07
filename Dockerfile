# Stage 1: Build and test
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copy and restore dependencies
COPY *.sln ./
COPY pbuild-api/*.csproj ./pbuild-api/
COPY pbuild-api.Tests/*.csproj ./pbuild-api.Tests/
RUN dotnet restore

# Copy the entire project and test sources
COPY . . 

# Build the app
RUN dotnet build --configuration Release --no-restore

# Run tests with code coverage
RUN dotnet test pbuild-tests/pbuild-tests.csproj \
    --no-build --configuration Release \
    /p:CollectCoverage=true \
    /p:CoverletOutputFormat=cobertura \
    /p:CoverletOutput=TestResults/

# Publish the app
RUN dotnet publish pbuild-api/pbuild-api.csproj \
    --configuration Release \
    --output /app/publish

# Stage 2: Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "pbuild-api.dll"]
