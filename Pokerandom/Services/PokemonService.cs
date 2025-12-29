using Entities;
using Dtos;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public interface IPokemonService
    {
        Task<IEnumerable<PokemonDto>> GetAllPokemonsAsync();
        Task<PokemonDto?> GetPokemonByIdAsync(long id);
        Task<PokemonDto> CreatePokemonAsync(PokemonDto pokemonDto);
        Task<PokemonDto?> UpdatePokemonAsync(long id, PokemonDto pokemonDto);
        Task<bool> DeletePokemonAsync(long id);
        Task<PokemonDto?> GetRandomPokemonAsync();
    }

    public class PokemonService : IPokemonService
    {
        private readonly PokemonDbContext _context;

        public PokemonService(PokemonDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PokemonDto>> GetAllPokemonsAsync()
        {
            var pokemons = await _context.Pokemons.ToListAsync();
            return pokemons.Select(p => MapToDto(p));
        }

        public async Task<PokemonDto?> GetPokemonByIdAsync(long id)
        {
            var pokemon = await _context.Pokemons.FindAsync(id);
            return pokemon == null ? null : MapToDto(pokemon);
        }

        public async Task<PokemonDto> CreatePokemonAsync(PokemonDto pokemonDto)
        {
            var pokemon = MapToEntity(pokemonDto);
            pokemon.Id = 0; // L'ID sera auto-généré
            _context.Pokemons.Add(pokemon);
            await _context.SaveChangesAsync();
            return MapToDto(pokemon);
        }

        public async Task<PokemonDto?> UpdatePokemonAsync(long id, PokemonDto pokemonDto)
        {
            var pokemon = await _context.Pokemons.FindAsync(id);
            if (pokemon == null)
                return null;

            pokemon.Name = pokemonDto.Name;
            pokemon.Description = pokemonDto.Description;
            pokemon.Type1 = pokemonDto.Type1;
            pokemon.Type2 = pokemonDto.Type2;

            await _context.SaveChangesAsync();
            return MapToDto(pokemon);
        }

        public async Task<bool> DeletePokemonAsync(long id)
        {
            var pokemon = await _context.Pokemons.FindAsync(id);
            if (pokemon == null)
                return false;

            _context.Pokemons.Remove(pokemon);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PokemonDto?> GetRandomPokemonAsync()
        {
            var count = await _context.Pokemons.CountAsync();
            if (count == 0)
                return null;

            var random = new Random();
            var skip = random.Next(0, count);
            var pokemon = await _context.Pokemons.Skip(skip).FirstOrDefaultAsync();
            return pokemon == null ? null : MapToDto(pokemon);
        }

        private static PokemonDto MapToDto(PokemonEntity entity)
        {
            return new PokemonDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Type1 = entity.Type1,
                Type2 = entity.Type2
            };
        }

        private static PokemonEntity MapToEntity(PokemonDto dto)
        {
            return new PokemonEntity
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Type1 = dto.Type1,
                Type2 = dto.Type2
            };
        }
    }
}
