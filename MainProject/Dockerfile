FROM mcr.microsoft.com/dotnet/aspnet:3.1-focal AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1-focal AS build
WORKDIR /MainProject
COPY *.sln .
COPY MainProject/*.csproj ./MainProject/
COPY MainProject/. ./MainProject/

FROM build AS publish
WORKDIR "/MainProject"
RUN dotnet publish -c Release -o /MainProject/publish

FROM base AS final
WORKDIR /MainProject
COPY --from=publish /app/publish .

EXPOSE 5000/tcp
ENV ASPNETCORE_URLS http://*:5000

ENTRYPOINT ["dotnet", "MainProject.dll"]

MAINTAINER Hüseyin Örer <huseyinorer@yandex.com>