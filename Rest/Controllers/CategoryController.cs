using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rest.Dto;
using Rest.Interfaces;
using Rest.Models;

namespace Rest.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : Controller
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
    public IActionResult GetCategories()
    {
        var categories = _mapper.Map<List<CategoryDto>>( _categoryRepository.GetAll());
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(categories);
    }
    
    [HttpGet("{categoryId}")]
    [ProducesResponseType(200, Type = typeof(Category))]
    [ProducesResponseType(400)]
    public IActionResult GetCategoryById(int categoryId)
    {
        if (!_categoryRepository.Exists(categoryId))
            return NotFound();
        var category = _mapper.Map<CategoryDto>(_categoryRepository.GetById(categoryId));
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(category);
    }
    
    [HttpGet("pokemon/{categoryId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
    [ProducesResponseType(400)]
    public IActionResult GetPokemonByCategoryId(int categoryId)
    {
        var pokemons = _mapper.Map<List<PokemonDto>>(
            _categoryRepository.GetPokemonByCategory(categoryId));
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return Ok(pokemons);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateCategory([FromBody] CategoryDto categoryCreate)
    {
        if (categoryCreate == null)
            return BadRequest(ModelState);
        
        var category = _categoryRepository.GetAll()
            .Where(c => c.Name.Trim().ToUpper() == categoryCreate.Name.TrimEnd().ToUpper()).FirstOrDefault();
        
        if (category != null)
        {
            ModelState.AddModelError("", "Category already exists");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var categoryMap = _mapper.Map<Category>(categoryCreate);

        if (!_categoryRepository.Create(categoryMap))
        {
            ModelState.AddModelError("", "Something went wrong while savin");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }

    [HttpPut("{categoryId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto updatedCategory)
    {
        if (updatedCategory == null)
            return BadRequest(ModelState);
        
        if (categoryId != updatedCategory.Id)
            return BadRequest(ModelState);
        
        if (!_categoryRepository.Exists(categoryId))
            return NotFound();
        
        if (!ModelState.IsValid)
            return BadRequest();

        var categoryMap = _mapper.Map<Category>(updatedCategory);

        if (!_categoryRepository.Update(categoryMap))
        {
            ModelState.AddModelError("", "Something went wrong updating category");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    [HttpDelete("{categoryId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult DeleteCategory(int categoryId)
    {
        if (!_categoryRepository.Exists(categoryId))
            return NotFound();

        var categoryToDelete = _categoryRepository.GetById(categoryId);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!_categoryRepository.Delete(categoryToDelete))
        {
            ModelState.AddModelError("", "Something went wrong deleting category");
        }

        return NoContent();
    }
}