using Rest.Models;

namespace Rest.Interfaces;

public interface IPokemonRepository
{
    ICollection<Pokemon> GetAll();
    Pokemon GetById(int id);
    Pokemon GetByName(string name);
    decimal GetRating(int pokeId);
    bool Exists(int pokeId);
    bool Exists(string pokeName);
    bool Create(Pokemon pokemon);
    bool Update(Pokemon pokemon);
    bool Delete(Pokemon pokemon);
    bool Save();
}