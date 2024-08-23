
using PokeAPI.Data;
using PokeAPI.Modelos;
using PokeAPI.Modelos.Dtos;
using XSystem.Security.Cryptography;

namespace PokeAPI.Repositorio.IRepositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        public readonly PokemonContext _bd;

        public UsuarioRepositorio(PokemonContext bd)
        {
            this._bd = bd;
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

        public Task<UsuarioLoginRespuestaDTO> Login(UsuarioLoginDTO usuarioLoginDTO)
        {
            throw new NotImplementedException();
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
            byte[] data = System.Text.Encoding.UTF8.GetBytes(password);
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
