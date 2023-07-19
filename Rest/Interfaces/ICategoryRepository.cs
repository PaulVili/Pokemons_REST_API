using Rest.Dto;
using Rest.Models;

namespace Rest.Interfaces;

public interface ICategoryRepository
{
    ICollection<Category> GetAll();
    Category GetById(int id);
    ICollection<Pokemon> GetPokemonByCategory(int categoryId);
    bool Exists(int id);
    bool Create(Category category);
    bool Update(Category category);
    bool Delete(Category category);
    bool Save();
}