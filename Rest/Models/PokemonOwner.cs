namespace Rest.Models;

public class PokemonOwner
{
    public int PokemonId { get; set; }
    public int OwnerId { get; set; }
    //Связи к таблицам родителей
    public Pokemon Pokemon { get; set; }
    public Owner Owner { get; set; }
}