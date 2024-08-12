using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PokeAPI.Modelos
{
    public class Entrenador
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EntrenadorId { get; set; }
        [Required]
        public String Nombre { get; set; }
        public int Edad { get; set; }
        public String Region { get; set; }
        public int PokemonId { get; set; }
        [ForeignKey("PokemonId")]
        [MaxLength(6)]
        public virtual ICollection<Pokemon> ListaPokemon { get; set; }
    }
}
