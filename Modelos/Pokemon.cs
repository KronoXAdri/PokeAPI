using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PokeAPI.Modelos
{
    public class Pokemon
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PokemonId { get; set; }

        public String Nombre { get; set; }

        public String Region { get; set; }

        public bool TieneShiny { get; set; }
    }
}
