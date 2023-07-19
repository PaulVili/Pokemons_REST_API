using Rest.Models;

namespace Rest.Interfaces;

public interface IReviewerRepository
{
    ICollection<Reviewer> GetAll();
    Reviewer GetById(int id);
    ICollection<Review> GetReviewsByReviewer(int pokeId);
    bool Exists(int id);
    bool Create(Reviewer reviewer);
    bool Update(Reviewer reviewer);
    bool Delete(Reviewer reviewer);
    bool Save();
}