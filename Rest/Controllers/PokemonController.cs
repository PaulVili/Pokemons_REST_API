using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rest.Dto;
using Rest.Interfaces;
using Rest.Models;

namespace Rest.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PokemonController : Controller
{
    private readonly IPokemonRepository _pokemonRepository;
    private readonly IMapper _mapper;

    public PokemonController(IPokemonRepository pokemonRepository, IMapper mapper)
    {
        _pokemonRepository = pokemonRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
    public IActionResult GetPokemons()
    {
        var pokemons = _mapper.Map<List<PokemonDto>>( _pokemonRepository.GetAll());
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(pokemons);
    }

    [HttpGet("findById/{pokeId}")]
    [ProducesResponseType(200, Type = typeof(Pokemon))]
    [ProducesResponseType(400)]
    public IActionResult GetPokemonById(int pokeId)
    {
        if (!_pokemonRepository.Exists(pokeId))
            return NotFound();
        var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetById(pokeId));
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(pokemon);
    }

    [HttpGet("findByName/{pokeName}")]
    [ProducesResponseType(200, Type = typeof(Pokemon))]
    [ProducesResponseType(400)]
    public IActionResult GetPokemonByName(string pokeName)
    {
        if (!_pokemonRepository.Exists(pokeName))
            return NotFound();
        var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetByName(pokeName));
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(pokemon);
    }

    [HttpGet("{pokeId}/rating")]
    [ProducesResponseType(200, Type = typeof(decimal))]
    [ProducesResponseType(400)]
    public IActionResult GetPokemonRating(int pokeId)
    {
        if (!_pokemonRepository.Exists(pokeId))
            return NotFound();
        var rating = _pokemonRepository.GetRating(pokeId);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(rating);
    }
    
    
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateOwner([FromBody] PokemonDto PokemonCreate)
    {
        if (PokemonCreate == null)
            return BadRequest(ModelState);

        var pokemon = _pokemonRepository.GetAll()
            .Where(c => c.Name.Trim().ToUpper() == PokemonCreate.Name.Trim().ToUpper()).FirstOrDefault();

        if (pokemon != null)
        {
            ModelState.AddModelError("", "Pokemon already exists");
            return StatusCode(422, ModelState);
        }
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var pokemonMap = _mapper.Map<Pokemon>(PokemonCreate);

        if (!_pokemonRepository.Create(pokemonMap))
        {
            ModelState.AddModelError("", "Something went wrong while savin");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }

    [HttpPut("{pokemonId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult UpdateOwner(int pokemonId, [FromBody] PokemonDto pokemonUpdate)
    {
        if (pokemonUpdate == null)
            return BadRequest(ModelState);

        if (pokemonId != pokemonUpdate.Id)
            return BadRequest(ModelState);

        if (!_pokemonRepository.Exists(pokemonId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        var pokemonMap = _mapper.Map<Pokemon>(pokemonUpdate);

        if (!_pokemonRepository.Update(pokemonMap))
        {
            ModelState.AddModelError("", "Something went wrong updating category");
            return StatusCode(500, ModelState);
        }
        
        return NoContent();
    }

    [HttpDelete("{pokemonId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult DeleteOwner(int pokemonId)
    {
        if (!_pokemonRepository.Exists(pokemonId))
            return NotFound();

        var pokemonToDelete = _pokemonRepository.GetById(pokemonId);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!_pokemonRepository.Delete(pokemonToDelete))
        {
            ModelState.AddModelError("", "Something went wrong deleting category");
        }
        
        return NoContent();
    }
    
}