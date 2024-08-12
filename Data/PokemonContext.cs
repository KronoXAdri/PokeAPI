using Microsoft.EntityFrameworkCore;
using PokeAPI.Modelos;

namespace PokeAPI.Data
{
    public class PokemonContext : DbContext 
    {
        public PokemonContext(DbContextOptions<PokemonContext> options) : base(options)
        {
        }
        public DbSet<Pokemon> Pokemons { get; set; }
        public DbSet<Entrenador> Entrenadores { get; set; }
    }
}
