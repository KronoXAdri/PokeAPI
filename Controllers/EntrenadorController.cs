using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokeAPI.Modelos;
using PokeAPI.Modelos.Dtos;
using PokeAPI.Repositorio.IRepositorio;

namespace PokeAPI.Controllers
{
    [Route("api/Entrenador")]
    [ApiController]
    public class EntrenadorController : ControllerBase
    { 
        private readonly IEntrenadorRepositorio _enRepo;
        private readonly IMapper _mapper;

        public EntrenadorController(IEntrenadorRepositorio enRepo, IMapper mapper)
        {
            this._enRepo = enRepo;
            this._mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetPokemons()
        {
            var listaEntrenadores = _enRepo.GetEntrenadores();

            var listaEntrenadoresDTO = new List<EntrenadorDTO>();

            foreach (Entrenador entrenador in listaEntrenadores)
            {
                listaEntrenadoresDTO.Add(this._mapper.Map<EntrenadorDTO>(entrenador));
            }

            return Ok(listaEntrenadoresDTO);
        }

        [HttpGet("{EntrenadorId:int}", Name = "GetEntrenador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetEntrenador(int EntrenadorId)
        {
            Entrenador entrenador = _enRepo.GetEntrenador(EntrenadorId);

            if (entrenador == null)
            {
                return NotFound();
            }

            EntrenadorDTO entrenadorDTO = this._mapper.Map<EntrenadorDTO>(entrenador);

            return Ok(entrenadorDTO);
        }

        [HttpPost]
        [ProducesResponseType(201, Type =  typeof(EntrenadorDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearPokemon([FromBody] CrearEntrenadorDTO entrenadorDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (entrenadorDTO == null)
            {
                return BadRequest(ModelState);
            }

            if (_enRepo.ExisteEntrenador(entrenadorDTO.Nombre))
            {
                ModelState.AddModelError("", "El entrenador ya existe");

                return StatusCode(404, ModelState);
            }

            Entrenador entrenador = _mapper.Map<Entrenador>(entrenadorDTO);

            if (!_enRepo.CrearEntrenador(entrenador))
            {
                ModelState.AddModelError("", $"Algo salió mal guardando el pokemon: {entrenador.Nombre}");

                return StatusCode(404, ModelState);
            }


            return CreatedAtRoute("GetEntrenador", new { entrenadorId = entrenador.EntrenadorId }, entrenador);
        }

    }
}
