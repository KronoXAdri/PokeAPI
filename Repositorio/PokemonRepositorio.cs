
using PokeAPI.Data;
using PokeAPI.Modelos;
using PokeAPI.Repositorio.IRepositorio;

namespace PokeAPI.Repositorio
{
    public class PokemonRepositorio : IPokemonRepositorio 
    {
        private readonly PokemonContext _bd;
        
        public PokemonRepositorio(PokemonContext bd)
        {
            this._bd = bd;
        }

        public bool ActualizarPokemon(Pokemon pokemon)
        {
            _bd.Pokemons.Update(pokemon);

            return Guardar();
        }

        public bool BorrarPokemon(Pokemon pokemon)
        {
            _bd.Pokemons.Remove(pokemon);

            return Guardar();
        }

        public bool CrearPokemon(Pokemon pokemon)
        {
            _bd.Pokemons.Add(pokemon);

            return Guardar();
        }

        public bool ExistePokemon(int PokemonId)
        {
            return this._bd.Pokemons.Any<Pokemon>(pokemon => pokemon.PokemonId == PokemonId);
        }

        public bool ExistePokemon(string PokemonNombre)
        {
            return this._bd.Pokemons.Any<Pokemon>(pokemon => pokemon.Nombre.ToLower().Trim() == PokemonNombre.ToLower().Trim());
        }

        public ICollection<Pokemon> GetPokemons()
        {
            return this._bd.Pokemons.OrderBy(pokemon => pokemon.Nombre).ToList<Pokemon>(); ;
        }

        public Pokemon GetPokemon(int PokemonId)
        {
            return this._bd.Pokemons.FirstOrDefault<Pokemon>(pokemon => pokemon.PokemonId == PokemonId);
        }

        public bool Guardar()
        {
            return this._bd.SaveChanges() >= 0 ? true : false;
        }
    }
}
