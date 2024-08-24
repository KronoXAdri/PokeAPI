using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PokeAPI.Modelos
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUsuario { get; set; }
        public String NombreUsuario { get; set; }
        public String Nombre { get; set; }
        public String Password { get; set; }
        public String Role { get; set; }
    }
}
