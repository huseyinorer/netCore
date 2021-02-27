FROM mcr.microsoft.com/dotnet/aspnet:3.1-focal AS base
WORKDIR ./

FROM mcr.microsoft.com/dotnet/sdk:3.1-focal AS build
WORKDIR ./
COPY *.sln .
COPY MainProject/*.csproj ./MainProject/
COPY MainProject/. ./MainProject/

FROM build AS publish
WORKDIR "/app/MainProject"
RUN dotnet publish -c Release -o ./publish

FROM base AS final
WORKDIR /MainProject
COPY --from=publish ./publish .

EXPOSE 5000/tcp
ENV ASPNETCORE_URLS http://*:5000

ENTRYPOINT ["dotnet", "MainProject.dll"]

MAINTAINER Hüseyin Örer <huseyinorer@yandex.com>