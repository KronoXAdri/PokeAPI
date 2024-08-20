
using System.ComponentModel.DataAnnotations;

namespace PokeAPI.Modelos.Dtos
{
    public class CrearEntrenadorDTO
    {
        public String Nombre { get; set; }
        public int Edad { get; set; }
        public String Region { get; set; }
        public string RutaImagen { get; set; }
        public enum CrearEspecialidad { Fuego, Psiquico, Volador, Hada }
        public CrearEspecialidad TipoEspecialidad { get; set; }
        public int PokemonId { get; set; }
    }
}
