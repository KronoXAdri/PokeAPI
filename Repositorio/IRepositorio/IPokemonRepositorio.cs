using PokeAPI.Modelos;

namespace PokeAPI.Repositorio.IRepositorio
{
    public interface IPokemonRepositorio
    {
        ICollection<Pokemon> GetPokemons();
        Pokemon GetPoklemon(int PokemonId);
        bool ExistePokemon(int PokemonId);
        bool ExistePokemon(string PokemonNombre);
        bool CrearPokemon(Pokemon pokemon);
        bool ActualizarPokemon(Pokemon pokemon);
        bool BorrarPokemon(Pokemon pokemon);
        bool Guardar();
    }
}
