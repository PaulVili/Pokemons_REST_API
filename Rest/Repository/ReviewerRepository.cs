using AutoMapper;
using Rest.Data;
using Rest.Interfaces;
using Rest.Models;

namespace Rest.Repository;

public class ReviewerRepository : IReviewerRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public ReviewerRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public ICollection<Reviewer> GetAll()
    {
        return _context.Reviewers.ToList();
    }

    public Reviewer GetById(int id)
    {
        return _context.Reviewers.FirstOrDefault(rv => rv.Id == id);
    }

    public ICollection<Review> GetReviewsByReviewer(int pokeId)
    {
        return _context.Reviews.Where(r => r.Reviewer.Id == pokeId).ToList();
    }

    public bool Exists(int id)
    {
        return _context.Reviewers.Any(rv => rv.Id == id);
    }

    public bool Create(Reviewer reviewer)
    {
        _context.Add(reviewer);
        return Save();
    }

    public bool Update(Reviewer reviewer)
    {
        _context.Update(reviewer);
        return Save();
    }

    public bool Delete(Reviewer reviewer)
    {
        _context.Remove(reviewer);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }
}