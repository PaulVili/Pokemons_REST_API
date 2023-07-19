namespace Rest.Models;

public class Review
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }

    public int Rating { get; set; }
    //Связи к таблицам родителей
    public Reviewer Reviewer { get; set; }
    public Pokemon Pokemon { get; set; }
}