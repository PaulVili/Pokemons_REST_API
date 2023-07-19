using AutoMapper;
using Rest.Data;
using Rest.Interfaces;
using Rest.Models;

namespace Rest.Repository;

public class CountryRepository : ICountryRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public CountryRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public ICollection<Country> GetAll()
    {
        return _context.Countries.ToList();
    }

    public Country GetById(int id)
    {
        return _context.Countries.FirstOrDefault(c => c.Id == id);
    }

    public Country GetByOwner(int ownerId)
    {
        return _context.Owners.Where(o => o.Id == ownerId).Select(c => c.Country).FirstOrDefault();
    }

    public ICollection<Owner> GetAllOwnersFromCountry(int countryId)
    {
        return _context.Owners.Where(c => c.Country.Id == countryId).ToList();
    }

    public bool Exists(int id)
    {
        return _context.Countries.Any(c => c.Id == id);
    }

    public bool Create(Country country)
    {
        _context.Add(country);
        return Save();
    }

    public bool Update(Country country)
    {
        _context.Update(country);
        return Save();
    }

    public bool Delete(Country country)
    {
        _context.Remove(country);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }
}