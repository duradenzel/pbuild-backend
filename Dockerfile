FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY . .

RUN dotnet restore

RUN dotnet publish pbuild-api/pbuild-api.csproj \
    --configuration Release \
    --no-restore \
    --output /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5286
ENV ASPNETCORE_ENVIRONMENT=Production

LABEL com.centurylinklabs.watchtower.enable=true

EXPOSE 5286

ENTRYPOINT ["dotnet", "pbuild-api.dll"]