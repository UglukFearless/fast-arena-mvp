FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /backend_app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /backend_src
COPY ["FastArena.sln", "."]
COPY ["src/FastArena.Core/FastArena.Core.csproj", "src/FastArena.Core/"]
COPY ["src/FastArena.Dal/FastArena.Dal.csproj", "src/FastArena.Dal/"]
COPY ["src/FastArena.WebApi/FastArena.WebApi.csproj", "src/FastArena.WebApi/"]
COPY ["src/FastArena.WebHost/FastArena.WebHost.csproj", "src/FastArena.WebHost/"]
RUN dotnet restore "/backend_src/src/FastArena.WebHost/FastArena.WebHost.csproj"
COPY . .
WORKDIR "/backend_src/src/FastArena.WebHost/"
RUN dotnet build "FastArena.WebHost.csproj" -c Release -o /backend_app/build

FROM build AS publish
RUN dotnet publish "FastArena.WebHost.csproj" -r linux-x64 -c Release -o /backend_app/publish

FROM base AS final
WORKDIR /backend_app
COPY --from=publish /backend_app/publish .
RUN apt-get update && apt-get -y install curl

ENTRYPOINT ["dotnet", "FastArena.WebHost.dll"]