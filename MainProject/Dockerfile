FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0.102-ca-patch-buster-slim AS build
WORKDIR /app

COPY MainProject/*.csproj ./MainProject/

FROM build AS publish
WORKDIR "/app/MainProject"
RUN dotnet publish -c Release -o /app/publish /p:EnvironmentName=Production /p:CI=true

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS http://+:5000
EXPOSE 5000

ENTRYPOINT ["dotnet", "MainProject.dll"]

MAINTAINER Hüseyin Örer <huseyinorer@yandex.com>