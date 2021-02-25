FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /netcore

FROM mcr.microsoft.com/dotnet/sdk:5.0.102-ca-patch-buster-slim AS build
WORKDIR /netcore

COPY MainProject/*.csproj ./MainProject/

FROM build AS publish
WORKDIR "/netcore/MainProject"
RUN dotnet publish -c Release -o /netcore/MainProject/publish

FROM base AS final
WORKDIR /netcore
COPY --from=publish /netcore/MainProject/publish .

ENV ASPNETCORE_URLS http://+:5000
EXPOSE 5000

ENTRYPOINT ["dotnet", "MainProject.dll"]

MAINTAINER Hüseyin Örer <huseyinorer@yandex.com>