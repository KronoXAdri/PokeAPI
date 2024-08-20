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

        public string RutaImagen { get; set; }
        public enum Especialidad { Fuego, Psiquico, Volador, Hada }
        public Especialidad TipoEspecialidad { get; set; }
        public DateTime FechaCreacion { get; set; }

        //Relacion con pokemon
        public int PokemonId { get; set; }
        [ForeignKey("PokemonId")]
        public Pokemon Pokemon { get; set; }
    }
}
