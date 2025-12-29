# Étape 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copier les fichiers de projet
COPY ["Api/Api.csproj", "Api/"]
COPY ["Entities/Entities.csproj", "Entities/"]
COPY ["Services/Services.csproj", "Services/"]
COPY ["Dtos/Dtos.csproj", "Dtos/"]
COPY ["Shared/Shared.csproj", "Shared/"]

# Restaurer les dépendances
RUN dotnet restore "Api/Api.csproj"

# Copier le reste du code
COPY . .

# Build l'application
WORKDIR "/src/Api"
RUN dotnet build "Api.csproj" -c Release -o /app/build

# Étape 2: Publish
FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Étape 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copier les fichiers publiés
COPY --from=publish /app/publish .

# Exposer le port
EXPOSE 8080
EXPOSE 8081

# Variables d'environnement
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Point d'entrée
ENTRYPOINT ["dotnet", "Api.dll"]

