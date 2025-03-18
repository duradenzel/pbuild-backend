# Build stage to compile and test the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet build --configuration Release --no-restore
RUN dotnet test --no-build --configuration Release

# Runtime stage to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app
COPY --from=build /app/pbuild-api/bin/Release/net8.0/ .
ENTRYPOINT ["dotnet", "pbuild-api.dll"]
