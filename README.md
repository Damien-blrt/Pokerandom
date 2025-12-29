# API Pokemon

API REST pour gérer des Pokémons avec .NET 9.0

## Endpoints disponibles

- `GET /api/pokemon` - Récupère tous les Pokémons
- `GET /api/pokemon/{id}` - Récupère un Pokémon par son ID
- `GET /api/pokemon/random` - Récupère un Pokémon aléatoire
- `POST /api/pokemon` - Crée un nouveau Pokémon
- `PUT /api/pokemon/{id}` - Met à jour un Pokémon existant
- `DELETE /api/pokemon/{id}` - Supprime un Pokémon

## Tester l'API avec Swagger

1. Lancez l'API (en local ou via Docker)
2. Ouvrez votre navigateur et allez sur `http://localhost:8080/swagger`
3. Vous verrez l'interface Swagger avec tous les endpoints disponibles
4. Cliquez sur un endpoint pour voir les détails
5. Cliquez sur "Try it out" pour tester l'endpoint
6. Remplissez les paramètres si nécessaire et cliquez sur "Execute"

### Exemple : Créer un Pokémon

1. Dans Swagger UI, trouvez l'endpoint `POST /api/pokemon`
2. Cliquez sur "Try it out"
3. Modifiez le JSON dans le corps de la requête :
```json
{
  "id": 0,
  "name": "Pikachu",
  "description": "Pokémon électrique jaune",
  "type1": 4,
  "type2": 0
}
```
4. Cliquez sur "Execute"
5. Vous verrez la réponse avec le Pokémon créé (l'ID sera auto-généré)

## Déploiement avec Docker

### Build l'image Docker

Depuis le répertoire racine du projet (Pokerandom) :

```bash
docker build -t pokerandom-api .
```

### Lancer le conteneur

```bash
docker run -d -p 8080:8080 --name pokerandom-api pokerandom-api
```

L'API sera accessible sur `http://localhost:8080`

### Interface Swagger UI

L'interface Swagger UI est disponible pour tester l'API directement depuis votre navigateur :

- **URL Swagger UI** : `http://localhost:8080/swagger`
- **Documentation OpenAPI JSON** : `http://localhost:8080/swagger/v1/swagger.json`

Depuis l'interface Swagger, vous pouvez :
- Voir tous les endpoints disponibles
- Tester chaque endpoint directement
- Voir les modèles de données (DTOs)
- Consulter la documentation de chaque endpoint

## Configuration

La base de données SQLite est créée automatiquement au premier démarrage. Le fichier `pokemon.db` sera créé dans le répertoire de l'application.

Pour changer la chaîne de connexion, modifiez `appsettings.json` :

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=pokemon.db"
  }
}
```

## Structure du projet

- **Api** - Contrôleurs et configuration de l'API
- **Services** - Logique métier
- **Entities** - Modèles de données et DbContext
- **Dtos** - Objets de transfert de données
- **Shared** - Types partagés (enum TypePkm)

