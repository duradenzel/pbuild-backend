FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY . .

RUN dotnet tool install --global dotnet-sonarscanner

ENV PATH="${PATH}:/root/.dotnet/tools"

RUN dotnet restore pbuild.sln  # Restore the solution from the root

RUN dotnet build pbuild.sln --configuration Release --no-restore

RUN dotnet test pbuild-tests/pbuild-tests.csproj --no-build --configuration Release \
  --collect:"XPlat Code Coverage" --results-directory /app/TestResults

RUN dotnet publish pbuild-api/pbuild-api.csproj --configuration Release --no-build --output /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5286


LABEL com.centurylinklabs.watchtower.enable=true

EXPOSE 5286

ENTRYPOINT ["dotnet", "pbuild-api.dll"]