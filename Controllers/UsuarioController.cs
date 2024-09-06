using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeAPI.Modelos;
using PokeAPI.Modelos.Dtos;
using PokeAPI.Repositorio.IRepositorio;
using System.Net;

namespace PokeAPI.Controllers
{
    [Route("api/Usuarios")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepositorio _usRepo;
        private readonly IMapper _mapper;
        protected RespuestaApi _respuestaAPI;

        public UsuarioController(IUsuarioRepositorio usRepo, IMapper mapper)
        {
            this._usRepo = usRepo;
            this._mapper = mapper;
            this._respuestaAPI = new();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetUsuarios()
        {
            List<Usuario> listaUsuarios = _usRepo.GetUsuarios().ToList<Usuario>();

            List<UsuarioDTO> listaUsuariosDTO = new List<UsuarioDTO>();

            foreach (Usuario usuario in listaUsuarios)
            {
                listaUsuariosDTO.Add(this._mapper.Map<UsuarioDTO>(usuario));
            }

            return Ok(listaUsuariosDTO);
        }

        [HttpGet("{UsuarioId:int}", Name = "GetUsuario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUsuario(int UsuarioId)
        {
            Usuario usuario = _usRepo.GetUsuario(UsuarioId);

            if (usuario == null)
            {
                return NotFound();
            }

            UsuarioDTO usuarioDTO = this._mapper.Map<UsuarioDTO>(usuario);

            return Ok(usuarioDTO);
        }

        [HttpPost("Registro")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Registro([FromBody] UsuarioRegistroDTO usuarioRegistroDTO)
        {
            bool validarNombreUsuarioUnico = _usRepo.isUniqueUser(usuarioRegistroDTO.NombreUsuario);

            if (!validarNombreUsuarioUnico)
            {
                _respuestaAPI.StatusCode = HttpStatusCode.BadRequest;
                _respuestaAPI.IsSuccess = false;
                _respuestaAPI.ErrorMessages.Add("El nombre de usuario ya existe.");

                return BadRequest(_respuestaAPI);
            }

            Usuario usuario = await _usRepo.Registro(usuarioRegistroDTO);
            if (usuario == null)
            {
                _respuestaAPI.StatusCode = HttpStatusCode.BadRequest;
                _respuestaAPI.IsSuccess = false;
                _respuestaAPI.ErrorMessages.Add("Error en el registro.");

                return BadRequest(_respuestaAPI);
            }

            _respuestaAPI.StatusCode = HttpStatusCode.Created;
            _respuestaAPI.IsSuccess = true;
            return Ok(_respuestaAPI);
        }

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDTO usuarioLoginDTO)
        {
            UsuarioLoginRespuestaDTO usuarioLoginRespuestaDTO = await _usRepo.Login(usuarioLoginDTO);

            if (usuarioLoginRespuestaDTO.Usuario == null || String.IsNullOrEmpty(usuarioLoginRespuestaDTO.Token))
            {
                _respuestaAPI.StatusCode = HttpStatusCode.BadRequest;
                _respuestaAPI.IsSuccess = false;
                _respuestaAPI.ErrorMessages.Add("El nombre de usuario o password son incorrectos.");

                return BadRequest(_respuestaAPI);
            }

            _respuestaAPI.StatusCode = HttpStatusCode.OK;
            _respuestaAPI.IsSuccess = true;
            _respuestaAPI.Result = usuarioLoginRespuestaDTO;
            return Ok(_respuestaAPI);
        }

    }
}
