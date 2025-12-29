using Microsoft.AspNetCore.Mvc;
using Services;
using Dtos;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;
        private readonly ILogger<PokemonController> _logger;

        public PokemonController(IPokemonService pokemonService, ILogger<PokemonController> logger)
        {
            _pokemonService = pokemonService;
            _logger = logger;
        }

        /// <summary>
        /// Récupère tous les Pokémons
        /// </summary>
        /// <returns>Liste de tous les Pokémons</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<PokemonDto>>> GetAllPokemons()
        {
            try
            {
                var pokemons = await _pokemonService.GetAllPokemonsAsync();
                return Ok(pokemons);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des Pokémons");
                return StatusCode(500, "Une erreur est survenue lors de la récupération des Pokémons");
            }
        }

        /// <summary>
        /// Récupère un Pokémon par son ID
        /// </summary>
        /// <param name="id">ID du Pokémon</param>
        /// <returns>Le Pokémon correspondant</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PokemonDto>> GetPokemonById(long id)
        {
            try
            {
                var pokemon = await _pokemonService.GetPokemonByIdAsync(id);
                if (pokemon == null)
                    return NotFound($"Pokémon avec l'ID {id} introuvable");

                return Ok(pokemon);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération du Pokémon {Id}", id);
                return StatusCode(500, "Une erreur est survenue lors de la récupération du Pokémon");
            }
        }

        /// <summary>
        /// Récupère un Pokémon aléatoire
        /// </summary>
        /// <returns>Un Pokémon choisi aléatoirement</returns>
        [HttpGet("random")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PokemonDto>> GetRandomPokemon()
        {
            try
            {
                var pokemon = await _pokemonService.GetRandomPokemonAsync();
                if (pokemon == null)
                    return NotFound("Aucun Pokémon disponible");

                return Ok(pokemon);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération d'un Pokémon aléatoire");
                return StatusCode(500, "Une erreur est survenue lors de la récupération d'un Pokémon aléatoire");
            }
        }

        /// <summary>
        /// Crée un nouveau Pokémon
        /// </summary>
        /// <param name="pokemonDto">Données du Pokémon à créer</param>
        /// <returns>Le Pokémon créé</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PokemonDto>> CreatePokemon([FromBody] PokemonDto pokemonDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdPokemon = await _pokemonService.CreatePokemonAsync(pokemonDto);
                return CreatedAtAction(nameof(GetPokemonById), new { id = createdPokemon.Id }, createdPokemon);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création du Pokémon");
                return StatusCode(500, "Une erreur est survenue lors de la création du Pokémon");
            }
        }

        /// <summary>
        /// Met à jour un Pokémon existant
        /// </summary>
        /// <param name="id">ID du Pokémon à mettre à jour</param>
        /// <param name="pokemonDto">Nouvelles données du Pokémon</param>
        /// <returns>Le Pokémon mis à jour</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PokemonDto>> UpdatePokemon(long id, [FromBody] PokemonDto pokemonDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedPokemon = await _pokemonService.UpdatePokemonAsync(id, pokemonDto);
                if (updatedPokemon == null)
                    return NotFound($"Pokémon avec l'ID {id} introuvable");

                return Ok(updatedPokemon);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour du Pokémon {Id}", id);
                return StatusCode(500, "Une erreur est survenue lors de la mise à jour du Pokémon");
            }
        }

        /// <summary>
        /// Supprime un Pokémon
        /// </summary>
        /// <param name="id">ID du Pokémon à supprimer</param>
        /// <returns>Aucun contenu</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePokemon(long id)
        {
            try
            {
                var deleted = await _pokemonService.DeletePokemonAsync(id);
                if (!deleted)
                    return NotFound($"Pokémon avec l'ID {id} introuvable");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression du Pokémon {Id}", id);
                return StatusCode(500, "Une erreur est survenue lors de la suppression du Pokémon");
            }
        }
    }
}
