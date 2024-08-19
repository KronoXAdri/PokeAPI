
using AutoMapper;
using PokeAPI.Modelos;
using PokeAPI.Modelos.Dtos;

namespace PokeAPI.PokemonsMapper
{
    public class PokemonMapper : Profile
    {

        public PokemonMapper()
        {
            CreateMap<Pokemon, PokemonDTO>().ReverseMap();
            CreateMap<Pokemon, CrearPokemonDTO>().ReverseMap();
            CreateMap<Entrenador, EntrenadorDTO>().ReverseMap();
            CreateMap<Entrenador, CrearEntrenadorDTO>().ReverseMap();
        }
    }
}
