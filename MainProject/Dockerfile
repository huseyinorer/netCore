FROM mcr.microsoft.com/dotnet/aspnet:3.1-focal AS base
#WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1-focal AS build
#WORKDIR /app
COPY *.sln /app
COPY MainProject/*.csproj ./app/MainProject/
COPY MainProject/. ./app/MainProject/

FROM build AS publish
WORKDIR "/MainProject"
RUN dotnet publish -c Release -o /publish

FROM base AS final
WORKDIR /MainProject
COPY --from=publish /publish .

EXPOSE 5000/tcp
ENV ASPNETCORE_URLS http://*:5000

ENTRYPOINT ["dotnet", "MainProject.dll"]

MAINTAINER Hüseyin Örer <huseyinorer@yandex.com>