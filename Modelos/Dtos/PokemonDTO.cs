
using System.ComponentModel.DataAnnotations;

namespace PokeAPI.Modelos.Dtos
{
    public class PokemonDTO
    {
        public int PokemonId { get; set; }
        [Required(ErrorMessage = "El nombre el obligatorio")]
        [MaxLength(100, ErrorMessage = "El número máximo de caracteres es 100")]
        public String Nombre { get; set; }

        public String Region { get; set; }

        public bool TieneShiny { get; set; }
    }
}
