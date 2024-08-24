
using Microsoft.IdentityModel.Tokens;
using PokeAPI.Data;
using PokeAPI.Modelos;
using PokeAPI.Modelos.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XSystem.Security.Cryptography;

namespace PokeAPI.Repositorio.IRepositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        public readonly PokemonContext _bd;
        private String claveSecreta;

        public UsuarioRepositorio(PokemonContext bd, IConfiguration config)
        {
            this._bd = bd;
            this.claveSecreta = config.GetValue<String>("AppiSettings:Secreta");
        }

        public ICollection<Usuario> GetUsuarios()
        {
            return _bd.Usuarios.OrderBy(usuario => usuario.IdUsuario).ToList<Usuario>();
        }

        public Usuario GetUsuario(int usuarioId)
        {
            return _bd.Usuarios.FirstOrDefault<Usuario>(usuario => usuario.IdUsuario == usuarioId);
        }

        public bool isUniqueUser(string nombreUsuario)
        {
            Usuario usuario = _bd.Usuarios.FirstOrDefault<Usuario>(usuario => usuario.NombreUsuario == nombreUsuario);

            return (usuario == null) ? true : false;
        }

        public async Task<UsuarioLoginRespuestaDTO> Login(UsuarioLoginDTO usuarioLoginDTO)
        {
            String passwordEncriptado = obtenermd5(usuarioLoginDTO.Password);
            Usuario usuario = _bd.Usuarios.FirstOrDefault<Usuario>(
                usuario => usuario.NombreUsuario.ToLower() == usuarioLoginDTO.NombreUsuario.ToLower()
                && usuario.Password == passwordEncriptado
                );

            if(usuario == null)
            {
                return new UsuarioLoginRespuestaDTO()
                {
                    Token = "",
                    Usuario = null
                };
            }

            JwtSecurityTokenHandler manejadorToken = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(claveSecreta);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.NombreUsuario.ToString()),
                    new Claim(ClaimTypes.Role, usuario.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = manejadorToken.CreateToken(tokenDescriptor);

            UsuarioLoginRespuestaDTO usuarioLoginRespuestaDTO = new UsuarioLoginRespuestaDTO
            {
                Token = manejadorToken.WriteToken(token),
                Usuario = usuario
            };

            return usuarioLoginRespuestaDTO;
        }

        public async Task<Usuario> Registro(UsuarioRegistroDTO usuarioRegistroDTO)
        {
            String passwordEncriptado = obtenermd5(usuarioRegistroDTO.Password);

            Usuario usuario = new Usuario()
            {
                NombreUsuario = usuarioRegistroDTO.NombreUsuario,
                Password = passwordEncriptado,
                Nombre = usuarioRegistroDTO.Nombre,
                Role = usuarioRegistroDTO.Role
            };

            _bd.Usuarios.Add(usuario);
            await _bd.SaveChangesAsync();
 
            usuario.Password = passwordEncriptado;
            return usuario;
        }

        private String obtenermd5(String password)
        {
            MD5CryptoServiceProvider cryptoService = new MD5CryptoServiceProvider();
            byte[] data = Encoding.UTF8.GetBytes(password);
            data = cryptoService.ComputeHash(data);
            String passwordEncriptada = "";
            for (int i = 0; i < data.Length; i++)
            {
                passwordEncriptada += data[i].ToString("x2").ToLower();
            }

            return passwordEncriptada;
        }
    }
}
