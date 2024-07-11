FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

ARG BUILD_CONFIGURATION=Release

COPY . ./
RUN dotnet restore
RUN dotnet publish -c $BUILD_CONFIGURATION -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Davids.Game.Api.dll"]
