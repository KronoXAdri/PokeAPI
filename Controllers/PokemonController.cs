using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokeAPI.Modelos;
using PokeAPI.Modelos.Dtos;
using PokeAPI.Repositorio.IRepositorio;

namespace PokeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonRepositorio _ctRepo;
        private readonly IMapper _mapper;

        public PokemonController(IPokemonRepositorio ctRepo, IMapper mapper)
        {
            this._ctRepo = ctRepo;
            this._mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetPokemons()
        {
            var listaPokemon = _ctRepo.GetPokemons();

            var listaPokemonDTO = new List<PokemonDTO>();

            foreach (Pokemon pokemon in listaPokemon)
            {
                listaPokemonDTO.Add(this._mapper.Map<PokemonDTO>(pokemon));
            }

            return Ok(listaPokemonDTO);
        }

        [HttpGet("{PokemonId:int}", Name = "GetPokemon")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPokemon(int PokemonId)
        {
            Pokemon pokemon = _ctRepo.GetPokemon(PokemonId);

            if (pokemon == null)
            {
                return NotFound();
            }

            PokemonDTO pokemonDTO = this._mapper.Map<PokemonDTO>(pokemon);

            return Ok(pokemonDTO);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearPokemon([FromBody] CrearPokemonDTO pokemonDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(pokemonDTO == null)
            {
                return BadRequest(ModelState);
            }

            if (_ctRepo.ExistePokemon(pokemonDTO.Nombre))
            {
                ModelState.AddModelError("", "El pokemon ya existe");

                return StatusCode(404, ModelState);
            }

            Pokemon pokemon = _mapper.Map<Pokemon>(pokemonDTO);

            if (!_ctRepo.CrearPokemon(pokemon))
            {
                ModelState.AddModelError("", $"Algo salió mal guardando el pokemon: {pokemon.Nombre}");

                return StatusCode(404, ModelState);
            }


            return CreatedAtRoute("GetPokemon", new {pokemonId = pokemon.PokemonId}, pokemon);
        }

        [HttpPut("{PokemonId:int}", Name = "GetPokemon")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ActualizarCampoPokemon(int PokemonId, [FromBody] PokemonDTO pokemonDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pokemonDTO == null || pokemonDTO.PokemonId != PokemonId)
            {
                return BadRequest(ModelState);
            }

            Pokemon pokemon = _mapper.Map<Pokemon>(pokemonDTO);

            if (!_ctRepo.ActualizarPokemon(pokemon))
            {
                ModelState.AddModelError("", $"Algo salió mal actualizando el pokemon: {pokemon.Nombre}");

                return StatusCode(404, ModelState);
            }


            return NoContent();
        }
    }
}
