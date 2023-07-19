namespace Rest.Models;

public class Reviewer
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    //Связи к таблицам детей
    public ICollection<Review> Reviews { get; set; }
}