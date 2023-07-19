using AutoMapper;
using Rest.Data;
using Rest.Dto;
using Rest.Interfaces;
using Rest.Models;

namespace Rest.Repository;

public class CategoryRepository : ICategoryRepository

{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public CategoryRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public ICollection<Category> GetAll()
    {
        return _context.Categories.ToList();
    }

    public Category GetById(int id)
    {
        return _context.Categories.Where(e => e.Id == id).FirstOrDefault();
    }

    public ICollection<Pokemon> GetPokemonByCategory(int categoryId)
    {
        return _context.PokemonCategories.Where(e => e.CategoryId == categoryId).Select(c => c.Pokemon).ToList();
    }

    public bool Exists(int id)
    {
        return _context.Categories.Any(p => p.Id == id);
    }

    public bool Create(Category category)
    {
        _context.Add(category);   
        return Save();
    }

    public bool Update(Category category)
    {
        _context.Update(category);
        return Save();
    }

    public bool Delete(Category category)
    {
        _context.Remove(category);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }
}