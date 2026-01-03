using Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
