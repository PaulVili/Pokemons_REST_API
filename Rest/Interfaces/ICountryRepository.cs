using Rest.Models;

namespace Rest.Interfaces;

public interface ICountryRepository
{
    ICollection<Country> GetAll();
    Country GetById(int id);
    Country GetByOwner(int ownerId);
    ICollection<Owner> GetAllOwnersFromCountry(int countryId);
    bool Exists(int id);
    bool Create(Country country);
    bool Update(Country country);
    bool Delete(Country country);
    bool Save();
}