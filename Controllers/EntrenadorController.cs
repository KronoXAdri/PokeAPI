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
        public IActionResult GetEntrenadores()
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

        [HttpPut("{EntrenadorId:int}", Name = "ActualizarCampoEntrenador")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ActualizarCampoEntrenador(int EntrenadorId, [FromBody] EntrenadorDTO entrenadorDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (entrenadorDTO == null || entrenadorDTO.EntrenadorId != EntrenadorId)
            {
                return BadRequest(ModelState);
            }

            Entrenador entrenador = _mapper.Map<Entrenador>(entrenadorDTO);

            if(!_enRepo.ExisteEntrenador(entrenador.EntrenadorId))
            {
                return NotFound($"No se ha encontrado el entrenador con el ID {entrenador.EntrenadorId} ");
            }

            if (!_enRepo.ActualizarEntrenador(entrenador))
            {
                ModelState.AddModelError("", $"Algo salió mal actualizando el pokemon: {entrenador.Nombre}");

                return StatusCode(500, ModelState);
            }


            return NoContent();
        }

        [HttpDelete("{EntrenadorId:int}", Name = "BorrarEntrenador")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult BorrarEntrenador(int EntrenadorId)
        {
            if (!_enRepo.ExisteEntrenador(EntrenadorId))
            {
                return NotFound();
            }

            Entrenador entrenador = _enRepo.GetEntrenador(EntrenadorId);

            if (!_enRepo.BorrarEntrenador(entrenador))
            {
                ModelState.AddModelError("", $"Algo salio mal borrando el registro {entrenador.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpGet("GetPokemonEntrenador/{entrenadorId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPokemonEntrenador(int pokemonId)
        {
            List<Entrenador> listaEntrenadores = _enRepo.GetEntrenadoresByPokemon(pokemonId).ToList<Entrenador>();

            if (listaEntrenadores == null)
            {
                return NotFound();
            }

            List<EntrenadorDTO> listaEntrenadoresDTO = new List<EntrenadorDTO>();

            foreach (Entrenador entrenador in listaEntrenadores)
            {
                EntrenadorDTO entrenadorDTO = _mapper.Map<EntrenadorDTO>(entrenador);

                listaEntrenadoresDTO.Add(entrenadorDTO);
            }

            return Ok(listaEntrenadoresDTO);
        }

        [HttpGet("BuscarEntrenador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult BuscarEntrenador(String nombreEntrenador)
        {
            try
            {
                List<Entrenador> listaEntrenadores = _enRepo.BuscarTipoEntrenador(nombreEntrenador).ToList<Entrenador>();

                if(listaEntrenadores.Any<Entrenador>())
                {
                    return Ok(listaEntrenadores);
                }

                return NotFound();
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

    }
}
