using AutoMapper;
using Rest.Data;
using Rest.Interfaces;
using Rest.Models;

namespace Rest.Repository;

public class PokemonRepository : IPokemonRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public PokemonRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public Pokemon GetById(int id)
    {
        return _context.Pokemons.Where(p => p.Id == id).FirstOrDefault();
    }
    
    public Pokemon GetByName(string name)
    {
        return _context.Pokemons.Where(p => p.Name == name).FirstOrDefault();
    }

    public decimal GetRating(int pokeId)
    {
        var review = _context.Reviews.Where(p => p.Id == pokeId);
        if (!review.Any())
        {
            return 0;
        }

        return ((decimal) review.Sum(r => r.Rating) / review.Count());
    }
    public ICollection<Pokemon> GetAll()
    {
        return _context.Pokemons.ToList();
    }

    public bool Exists(int pokeId)
    {
        return _context.Pokemons.Any(p => p.Id == pokeId);
    }
    public bool Exists(string pokeName)
    {
        return _context.Pokemons.Any(p => p.Name == pokeName);
    }

    public bool Create(Pokemon pokemon)
    {
        _context.Add(pokemon);
        return Save();
    }

    public bool Update(Pokemon pokemon)
    {
        _context.Update(pokemon);
        return Save();
    }

    public bool Delete(Pokemon pokemon)
    {
        _context.Remove(pokemon);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }
}