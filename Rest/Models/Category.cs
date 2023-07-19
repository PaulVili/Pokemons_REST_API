namespace Rest.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    //Связи к таблицам детей
    public ICollection<PokemonCategory> PokemonCategories { get; set; }
}