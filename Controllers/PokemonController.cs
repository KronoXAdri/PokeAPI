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
    }
}
