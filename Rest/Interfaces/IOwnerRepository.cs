using Rest.Dto;
using Rest.Models;

namespace Rest.Interfaces;

public interface IOwnerRepository
{
    ICollection<Owner> GetAll();
    Owner GetById(int id);
    ICollection<Owner> GetOwnerOfAPokemon(int pokeId);
    ICollection<Pokemon> GetPokemonByOwner(int ownerId);
    bool Exists(int id);
    bool Create(Owner owner);
    bool Update(Owner owner);
    bool Delete(Owner owner);
    bool Save();
}