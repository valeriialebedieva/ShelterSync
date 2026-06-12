FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ShelterSync.sln .
COPY ShelterSync/ShelterSync.csproj ShelterSync/
RUN dotnet restore

COPY . .
WORKDIR /src/ShelterSync
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Render sets PORT (often 10000)
ENV ASPNETCORE_URLS=http://0.0.0.0:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "ShelterSync.dll"]