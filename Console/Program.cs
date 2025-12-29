
using Entities;
using Stub;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace App
{
    public class Programme
    {
        static void Main(string[] args)
        {
            //using (var context = new PokemonDbContext())
            //{


            //    if (!context.Heros.Any())
            //    {
            //        context.Heros.Add(new Hero { Id = 1, Name = "Sage" });
            //        context.Heros.Add(new Hero { Id = 2, Name = "Omen" });
            //        context.Heros.Add(new Hero { Id = 3, Name = "Breach" });
            //        context.SaveChanges();
            //        Console.WriteLine("Héros ajoutés !");
            //    }




            //    foreach (var hero in context.Heros)
            //    {
            //        Console.WriteLine($"{hero.Id} - {hero.Name}");
            //    }
            //}
            var options = new DbContextOptionsBuilder<PokemonDbContext>()
                            .UseSqlite("Data Source=valorant.db")
                            .Options;
            using (var context = new StubbedContext(options))
            {
                context.Database.EnsureCreated();
                foreach (var pokemon in context.Pokemons)
                {
                    Console.WriteLine($"{pokemon.Id} - {pokemon.Name}");
                }
                Console.WriteLine();

                //3 pokemon aléatoires
                var randomPokemons = context.Pokemons
                    .OrderBy(p => EF.Functions.Random())
                    .Take(3)
                    .ToList();
                Console.WriteLine("Pokémons aléatoires :");
                foreach (var pokemon in randomPokemons)
                {
                    Console.WriteLine($"{pokemon.Id} - {pokemon.Name}");
                }



            }

        }
    }
}
