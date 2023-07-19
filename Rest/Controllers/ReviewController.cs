using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rest.Dto;
using Rest.Interfaces;
using Rest.Models;

namespace Rest.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewController : Controller
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IReviewerRepository _reviewerRepository;
    private readonly IMapper _mapper;

    public ReviewController(IReviewRepository reviewRepository, IReviewerRepository reviewerRepository, IMapper mapper)
    {
        _reviewRepository = reviewRepository;
        _reviewerRepository = reviewerRepository;
        _mapper = mapper;
    }
    
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
    public IActionResult GetAllReviews()
    {
        var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetAll());
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(reviews);
    } 
    
    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = typeof(Review))]
    [ProducesResponseType(400)]
    public IActionResult GetReviewById(int id)
    {
        if (!_reviewRepository.Exists(id))
            return NotFound();
        var review = _mapper.Map<OwnerDto>(_reviewRepository.GetById(id));
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(review);
    }
    
    [HttpGet("pokemon/{pokeId}")]
    [ProducesResponseType(200, Type = typeof(Review))]
    [ProducesResponseType(400)]
    public IActionResult GetReviewsForAPokemon(int pokeId)
    {
        var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviewsOfAPokemon(pokeId));
        if (!ModelState.IsValid)
            return BadRequest();
        return Ok(reviews);
    }
    
    
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateReview([FromBody] ReviewDto reviewDtoCreate)
    {
        if (reviewDtoCreate == null)
            return BadRequest(ModelState);

        var review = _reviewRepository.GetAll()
            .Where(c => c.Title.Trim().ToUpper() == reviewDtoCreate.Title.Trim().ToUpper()).FirstOrDefault();

        if (review != null)
        {
            ModelState.AddModelError("", "Review already exists");
            return StatusCode(422, ModelState);
        }
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var reviewMap = _mapper.Map<Review>(reviewDtoCreate);

        if (!_reviewRepository.Create(reviewMap))
        {
            ModelState.AddModelError("", "Something went wrong while savin");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }

    [HttpPut("{reviewId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDto reviewUpdate)
    {
        if (reviewUpdate == null)
            return BadRequest(ModelState);

        if (reviewId != reviewUpdate.Id)
            return BadRequest(ModelState);

        if (!_reviewRepository.Exists(reviewId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        var reviewMap = _mapper.Map<Review>(reviewUpdate);

        if (!_reviewRepository.Update(reviewMap))
        {
            ModelState.AddModelError("", "Something went wrong updating category");
            return StatusCode(500, ModelState);
        }
        
        return NoContent();
    }

    [HttpDelete("{reviewId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult DeleteReview(int reviewId)
    {
        if (!_reviewRepository.Exists(reviewId))
            return NotFound();

        var reviewToDelete = _reviewRepository.GetById(reviewId);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!_reviewRepository.Delete(reviewToDelete))
        {
            ModelState.AddModelError("", "Something went wrong deleting category");
        }
        
        return NoContent();
    }
    
    [HttpDelete("/DeleteReviewsByReviewer/{reviewerId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult DeleteReviewsByReviewer(int reviewerId)
    {
        if (!_reviewerRepository.Exists(reviewerId))
            return NotFound();

        var reviewsToDelete = _reviewerRepository.GetReviewsByReviewer(reviewerId).ToList();
        if (!ModelState.IsValid)
            return BadRequest();

        if (!_reviewRepository.DeleteReviews(reviewsToDelete))
        {
            ModelState.AddModelError("", "error deleting reviews");
            return StatusCode(500, ModelState);
        }
        return NoContent();
    }
    
}