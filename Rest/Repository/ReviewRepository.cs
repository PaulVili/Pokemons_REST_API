using AutoMapper;
using Rest.Data;
using Rest.Interfaces;
using Rest.Models;

namespace Rest.Repository;

public class ReviewRepository : IReviewRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public ReviewRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public ICollection<Review> GetAll()
    {
        return _context.Reviews.ToList();
    }

    public Review GetById(int id)
    {
        return _context.Reviews.Where(r => r.Id == id).FirstOrDefault();
    }

    public ICollection<Review> GetReviewsOfAPokemon(int pokeId)
    {
        return _context.Reviews.Where(r => r.Pokemon.Id == pokeId).ToList();
    }

    public bool Exists(int id)
    {
        return _context.Reviews.Any(r => r.Id == id);
    }

    public bool Create(Review review)
    {
        _context.Add(review);
        return Save();
    }

    public bool Update(Review review)
    {
        _context.Update(review);
        return Save();
    }

    public bool Delete(Review review)
    {
        _context.Remove(review);
        return Save();
    }

    public bool DeleteReviews(List<Review> reviews)
    {
        _context.RemoveRange(reviews);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }
}