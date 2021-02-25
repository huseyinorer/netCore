FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /MainProject

FROM mcr.microsoft.com/dotnet/sdk:5.0.102-ca-patch-buster-slim AS build
WORKDIR /MainProject

COPY MainProject/*.csproj ./MainProject/

FROM build AS publish
WORKDIR "/MainProject"
RUN dotnet publish -c Release -o /publish

FROM base AS final
WORKDIR /MainProject
COPY --from=publish /MainProject/publish .

ENV ASPNETCORE_URLS http://+:5000
EXPOSE 5000

ENTRYPOINT ["dotnet", "MainProject.dll"]

MAINTAINER Hüseyin Örer <huseyinorer@yandex.com>