FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY pbuild-backend.sln .  
COPY pbuild-api/*.csproj pbuild-api/  
COPY pbuild-business/*.csproj pbuild-business/  
COPY pbuild-data/*.csproj pbuild-data/  
COPY pbuild-domain/*.csproj pbuild-domain/  
COPY pbuild-tests/*.csproj pbuild-tests/  

RUN dotnet restore

COPY . .
RUN dotnet build --configuration Release --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "pbuild-api.dll"]
