using AutoMapper;
using Rest.Data;
using Rest.Interfaces;
using Rest.Models;

namespace Rest.Repository;

public class OwnerRepository : IOwnerRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public OwnerRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public ICollection<Owner> GetAll()
    {
        return _context.Owners.ToList();
    }

    public Owner GetById(int id)
    {
        return _context.Owners.FirstOrDefault(o => o.Id == id);
    }

    public ICollection<Owner> GetOwnerOfAPokemon(int pokeId)
    {
        return _context.PokemonOwners.Where(p => p.Pokemon.Id == pokeId).Select(o => o.Owner).ToList();
    }

    public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
    {
        return _context.PokemonOwners.Where(p => p.Owner.Id == ownerId).Select(p => p.Pokemon).ToList();
    }

    public bool Exists(int id)
    {
        return _context.Owners.Any(o => o.Id == id);
    }

    public bool Create(Owner owner)
    {
        _context.Add(owner);
        return Save();
    }

    public bool Update(Owner owner)
    {
        _context.Update(owner);
        return Save();
    }

    public bool Delete(Owner owner)
    {
        _context.Remove(owner);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }
}