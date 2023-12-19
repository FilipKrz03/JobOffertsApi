FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["JobOfferService/JobOffersService.csproj", "JobOfferService/"]
COPY ["JobOffersApiCore/JobOffersApiCore.csproj", "JobOffersApiCore/"]
RUN dotnet restore "JobOfferService/JobOffersService.csproj"
COPY . .
WORKDIR "/src/JobOfferService"
RUN dotnet build "JobOffersService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JobOffersService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JobOffersService.dll"]