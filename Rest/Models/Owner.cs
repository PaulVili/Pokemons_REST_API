namespace Rest.Models;

public class Owner
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Gym { get; set; }
    //Связи к таблицам родителей
    public Country Country { get; set; }
    //Связи к таблицам детей
    public ICollection<PokemonOwner> PokemonOwners { get; set; }
}