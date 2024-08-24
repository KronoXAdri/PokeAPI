using System.ComponentModel.DataAnnotations;

namespace PokeAPI.Modelos.Dtos
{
    public class UsuarioRegistroDTO
    {
        [Required (ErrorMessage = "El nombre del usuario es obligatorio")]
        public String NombreUsuario { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public String Nombre { get; set; }
        [Required(ErrorMessage = "El password es obligatorio")]
        public String Password { get; set; }
        public String Role { get; set; }
    }
}
