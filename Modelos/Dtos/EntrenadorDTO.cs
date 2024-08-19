
namespace PokeAPI.Modelos.Dtos
{
    public class EntrenadorDTO
    {
        public int EntrenadorId { get; set; }
        public String Nombre { get; set; }
        public int Edad { get; set; }
        public String Region { get; set; }
        public string RutaImagen { get; set; }
        public enum Especialidad { Fuego, Psiquico, Volador, Hada }
        public Especialidad TipoEspecialidad { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int PokemonId { get; set; }
    }
}
