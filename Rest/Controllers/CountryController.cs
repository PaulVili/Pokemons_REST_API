using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Rest.Dto;
using Rest.Interfaces;
using Rest.Models;

namespace Rest.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountryController : Controller
{
    private readonly ICountryRepository _countryRepository;
    private readonly IMapper _mapper;

    public CountryController(ICountryRepository countryRepository, IMapper mapper)
    {
        _countryRepository = countryRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
    public IActionResult GetAllCountries()
    {
        var countries = _mapper.Map<List<CountryDto>>(_countryRepository.GetAll());
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(countries);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = typeof(Country))]
    [ProducesResponseType(400)]
    public IActionResult GetCountryById(int id)
    {
        if (!_countryRepository.Exists(id))
            return NotFound();
        var country = _mapper.Map<CountryDto>(_countryRepository.GetById(id));
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(country);
    }

    [HttpGet("owners/{ownerId}")]
    [ProducesResponseType(200, Type = typeof(Country))]
    [ProducesResponseType(400)]
    public IActionResult GetCountryByOwner(int ownerId)
    {
        var country = _mapper.Map<CountryDto>(_countryRepository.GetByOwner(ownerId));
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(country);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateCountry([FromBody] CountryDto countryCreate)
    {
        if (countryCreate == null)
            return BadRequest(ModelState);

        var country = _countryRepository.GetAll()
            .Where(c => c.Name.Trim().ToUpper() == countryCreate.Name.Trim().ToUpper()).FirstOrDefault();

        if (country != null)
        {
            ModelState.AddModelError("", "Country already exists");
            return StatusCode(422, ModelState);
        }
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var countryMap = _mapper.Map<Country>(countryCreate);

        if (!_countryRepository.Create(countryMap))
        {
            ModelState.AddModelError("", "Something went wrong while savin");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }

    [HttpPut("{countryId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult UpdateCountry(int countryId, [FromBody] CountryDto countryUpdate)
    {
        if (countryUpdate == null)
            return BadRequest(ModelState);

        if (countryId != countryUpdate.Id)
            return BadRequest(ModelState);

        if (!_countryRepository.Exists(countryId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        var countryMap = _mapper.Map<Country>(countryUpdate);

        if (!_countryRepository.Update(countryMap))
        {
            ModelState.AddModelError("", "Something went wrong updating category");
            return StatusCode(500, ModelState);
        }
        
        return NoContent();
    }

    [HttpDelete("{countryId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult DeleteCountry(int countryId)
    {
        if (!_countryRepository.Exists(countryId))
            return NotFound();

        var countryToDelete = _countryRepository.GetById(countryId);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!_countryRepository.Delete(countryToDelete))
        {
            ModelState.AddModelError("", "Something went wrong deleting category");
        }
        
        return NoContent();
    }
}