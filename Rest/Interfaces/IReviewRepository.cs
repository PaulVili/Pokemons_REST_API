using Rest.Models;

namespace Rest.Interfaces;

public interface IReviewRepository
{
    ICollection<Review> GetAll();
    Review GetById(int id);
    ICollection<Review> GetReviewsOfAPokemon(int pokeId);
    bool Exists(int id);
    bool Create(Review review);
    bool Update(Review review);
    bool Delete(Review review);
    bool DeleteReviews(List<Review> reviews);
    bool Save();
}