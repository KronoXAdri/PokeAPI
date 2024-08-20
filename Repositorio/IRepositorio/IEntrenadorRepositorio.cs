using PokeAPI.Modelos;

namespace PokeAPI.Repositorio.IRepositorio
{
    public interface IEntrenadorRepositorio
    {
        ICollection<Entrenador> GetEntrenadores();
        ICollection<Entrenador> GetEntrenadoresByPokemon(int pokemonId);
        IEnumerable<Entrenador> BuscarTipoEntrenador(String nombreTipo);
        Entrenador GetEntrenador(int entrenadorId);
        bool ExisteEntrenador(int entrenadorId);
        bool ExisteEntrenador(string entrenadorNombre);
        bool CrearEntrenador(Entrenador entrenador);
        bool ActualizarEntrenador(Entrenador entrenador);
        bool BorrarEntrenador(Entrenador entrenador);
        bool Guardar();
    }
}
