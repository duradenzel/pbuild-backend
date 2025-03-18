FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app
COPY . . 
RUN dotnet restore
RUN dotnet build --configuration Release --no-restore
RUN dotnet test --no-build --configuration Release --collect:"XPlat Code Coverage"

RUN dotnet tool install --global coveralls.io

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app
COPY --from=build /app/pbuild-api/bin/Release/net8.0/ . 
ENTRYPOINT ["dotnet", "pbuild-api.dll"]
