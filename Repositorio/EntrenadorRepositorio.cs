
using Microsoft.EntityFrameworkCore;
using PokeAPI.Data;
using PokeAPI.Modelos;
using PokeAPI.Repositorio.IRepositorio;

namespace PokeAPI.Repositorio
{
    public class EntrenadorRepositorio : IEntrenadorRepositorio 
    {
        private readonly PokemonContext _bd;
        

        public EntrenadorRepositorio(PokemonContext bd)
        {
            this._bd = bd;
        }

        public bool ActualizarEntrenador(Entrenador entrenador)
        {
            entrenador.FechaCreacion = DateTime.Now;

            _bd.Entrenadores.Update(entrenador);

            return Guardar();
        }

        public bool BorrarEntrenador(Entrenador entrenador)
        {
            _bd.Entrenadores.Remove(entrenador);

            return Guardar();
        }

        public bool CrearEntrenador(Entrenador entrenador)
        {
            _bd.Entrenadores.Add(entrenador);

            return Guardar();
        }

        public bool ExisteEntrenador(int entrenadorId)
        {
            return this._bd.Entrenadores.Any<Entrenador>(entrenador => entrenador.EntrenadorId == entrenadorId);
        }

        public bool ExisteEntrenador(string entrenadorNombre)
        {
            return this._bd.Entrenadores.Any<Entrenador>(entrenador => entrenador.Nombre.ToLower().Trim() == entrenadorNombre.ToLower().Trim());
        }

        public ICollection<Entrenador> GetEntrenadores()
        {
            return this._bd.Entrenadores.OrderBy(entrenador => entrenador.Nombre).ToList<Entrenador>(); ;
        }

        public Entrenador GetEntrenador(int entrenadorId)
        {
            return this._bd.Entrenadores.FirstOrDefault<Entrenador>(entrenador => entrenador.EntrenadorId == entrenadorId);
        }

        public ICollection<Entrenador> GetEntrenadoresByPokemon(int pokemonId)
        {
            return _bd.Entrenadores.Include(po => po.Pokemon).Where<Entrenador>(en => en.PokemonId == pokemonId).ToList<Entrenador>();
        }

        public IEnumerable<Entrenador> BuscarTipoEntrenador(string nombreTipo)
        {
            IQueryable<Entrenador> query = _bd.Entrenadores;

            if (!string.IsNullOrEmpty(nombreTipo))
            {
                query = query.Where(e => e.Nombre.Contains(nombreTipo) || e.Region.Contains(nombreTipo));
            }

            return query.ToList<Entrenador>();
        }

        public bool Guardar()
        {
            return this._bd.SaveChanges() >= 0 ? true : false;
        }
    }
}
