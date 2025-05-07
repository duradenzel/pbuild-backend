FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copy solution and all source files
COPY . .

# Install tools for test coverage
RUN dotnet tool install --global dotnet-sonarscanner
ENV PATH="${PATH}:/root/.dotnet/tools"

# Restore and build the solution
RUN dotnet restore pbuild.sln
RUN dotnet build pbuild.sln --configuration Release --no-restore

# Run tests with code coverage
RUN dotnet test pbuild-tests/pbuild-tests.csproj \
    --no-build --configuration Release \
    --logger "trx;LogFileName=test_results.trx" \
    /p:CollectCoverage=true \
    /p:CoverletOutput=TestResults/ \
    /p:CoverletOutputFormat=cobertura

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /src/pbuild-api/bin/Release/net8.0/ . 
ENTRYPOINT ["dotnet", "pbuild-api.dll"]
