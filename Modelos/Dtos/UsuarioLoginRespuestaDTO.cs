
namespace PokeAPI.Modelos.Dtos
{
    public class UsuarioLoginRespuestaDTO
    {
        public UsuarioDatosDTO Usuario { get; set; }
        public String Role { get; set; }
        public String Token { get; set; }
    }
}
