#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["fake/Dii_MovieCatalogSvc.Fake/Dii_MovieCatalogSvc.Fake.csproj", "fake/Dii_MovieCatalogSvc.Fake/"]
RUN dotnet restore "fake/Dii_MovieCatalogSvc.Fake/Dii_MovieCatalogSvc.Fake.csproj"
COPY . .
WORKDIR "/src/fake/Dii_MovieCatalogSvc.Fake"
RUN dotnet build "Dii_MovieCatalogSvc.Fake.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dii_MovieCatalogSvc.Fake.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dii_MovieCatalogSvc.Fake.dll"]
