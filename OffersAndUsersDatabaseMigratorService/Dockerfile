#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["OffersAndUsersDatabaseMigratorService/OffersAndUsersDatabaseMigratorService.csproj", "OffersAndUsersDatabaseMigratorService/"]
COPY ["JobOffersApiCore/JobOffersApiCore.csproj", "JobOffersApiCore/"]
RUN dotnet restore "OffersAndUsersDatabaseMigratorService/OffersAndUsersDatabaseMigratorService.csproj"
COPY . .
WORKDIR "/src/OffersAndUsersDatabaseMigratorService"
RUN dotnet build "OffersAndUsersDatabaseMigratorService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OffersAndUsersDatabaseMigratorService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY wait-for-it.sh /app/wait-for-it.sh
RUN chmod +x /app/wait-for-it.sh

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OffersAndUsersDatabaseMigratorService.dll"]