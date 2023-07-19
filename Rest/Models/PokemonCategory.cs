namespace Rest.Models;

public class PokemonCategory
{
    public int PokemonId { get; set; }
    public int CategoryId { get; set; }
    //Связи к таблицам родителей
    public Pokemon Pokemon { get; set; }
    public Category Category { get; set; }
}