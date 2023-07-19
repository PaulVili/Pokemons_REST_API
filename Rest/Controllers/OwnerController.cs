using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rest.Dto;
using Rest.Interfaces;
using Rest.Models;

[Route("api/[controller]")]
[ApiController]
public class OwnerController : Controller
{
    private readonly IOwnerRepository _ownerRepository;
    private readonly IPokemonRepository _pokemonRepository;
    private readonly IMapper _mapper;

    public OwnerController(IOwnerRepository ownerRepository, IPokemonRepository pokemonRepository, IMapper mapper)
    {
        _ownerRepository = ownerRepository;
        _pokemonRepository = pokemonRepository;
        _mapper = mapper;
    }
    
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
    public IActionResult GetAllOwners()
    {
        var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetAll());
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(owners);
    } 
    
    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = typeof(Owner))]
    [ProducesResponseType(400)]
    public IActionResult GetOwnerById(int id)
    {
        if (!_ownerRepository.Exists(id))
            return NotFound();
        var owner = _mapper.Map<OwnerDto>(_ownerRepository.GetById(id));
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(owner);
    }
    
    [HttpGet("owners/{pokeId}")]
    [ProducesResponseType(200, Type = typeof(Owner))]
    [ProducesResponseType(400)]
    public IActionResult GetOwnerByPokemonById(int pokeId)
    {
        if (_pokemonRepository.Exists(pokeId))
            return NotFound();
        var owner = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwnerOfAPokemon(pokeId));
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(owner);
    }


    [HttpGet("pokemons/{ownerId}")]
    [ProducesResponseType(200, Type = typeof(Country))]
    [ProducesResponseType(400)]
    public IActionResult GetPokemonByOwner(int ownerId)
    {
        if (_ownerRepository.Exists(ownerId))
            return NotFound();
        var owner = _mapper.Map<List<PokemonDto>>(_ownerRepository.GetPokemonByOwner(ownerId));
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(owner);
    }
    
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateOwner([FromBody] OwnerDto OwnerCreate)
    {
        if (OwnerCreate == null)
            return BadRequest(ModelState);

        var owner = _ownerRepository.GetAll()
            .Where(c => c.LastName.Trim().ToUpper() == OwnerCreate.LastName.Trim().ToUpper()).FirstOrDefault();

        if (owner != null)
        {
            ModelState.AddModelError("", "Owner already exists");
            return StatusCode(422, ModelState);
        }
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var ownerMap = _mapper.Map<Owner>(OwnerCreate);

        if (!_ownerRepository.Create(ownerMap))
        {
            ModelState.AddModelError("", "Something went wrong while savin");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }

    [HttpPut("{ownerId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult UpdateOwner(int ownerId, [FromBody] CountryDto ownerUpdate)
    {
        if (ownerUpdate == null)
            return BadRequest(ModelState);

        if (ownerId != ownerUpdate.Id)
            return BadRequest(ModelState);

        if (!_ownerRepository.Exists(ownerId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        var ownerMap = _mapper.Map<Owner>(ownerUpdate);

        if (!_ownerRepository.Update(ownerMap))
        {
            ModelState.AddModelError("", "Something went wrong updating category");
            return StatusCode(500, ModelState);
        }
        
        return NoContent();
    }

    [HttpDelete("{ownerId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult DeleteOwner(int ownerId)
    {
        if (!_ownerRepository.Exists(ownerId))
            return NotFound();

        var ownerToDelete = _ownerRepository.GetById(ownerId);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!_ownerRepository.Delete(ownerToDelete))
        {
            ModelState.AddModelError("", "Something went wrong deleting category");
        }
        
        return NoContent();
    }
}