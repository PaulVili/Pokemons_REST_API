namespace Rest.Models;

public class Country
{
    public int Id { get; set; }
    public string Name { get; set; }
    //Связи к таблицам детей
    public ICollection<Owner> Owners { get; set; }
}