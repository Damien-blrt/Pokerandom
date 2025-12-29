using Entities;
using Microsoft.EntityFrameworkCore;
using Stub;

namespace Services
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(StubbedContext context)
        {
            // 1. Crée la base et les tables si elles n'existent pas
            // Note: EnsureCreated() insère les données HasData() 
            // UNIQUEMENT si la base est créée à cet instant précis.
            await context.Database.EnsureCreatedAsync();

            // 2. Sécurité : Si vous voulez forcer l'ajout au cas où EnsureCreated 
            // n'aurait pas mis les données (ex: base déjà existante mais vide)
            if (!await context.Pokemons.AnyAsync())
            {
                // Ici, on pourrait ajouter manuellement, mais avec 1025 pokémons, 
                // il vaut mieux supprimer la db pokemon.db et laisser EnsureCreated faire le job.
            }
        }
    }
}