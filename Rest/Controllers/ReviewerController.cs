using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rest.Dto;
using Rest.Interfaces;
using Rest.Models;

namespace Rest.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewerController : Controller
{
    private readonly IReviewerRepository _reviewerRepository;
    private readonly IMapper _mapper;

    public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
    {
        _reviewerRepository = reviewerRepository;
        _mapper = mapper;
    }
    
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
    public IActionResult GetAllReviewers()
    {
        var reviewers = _mapper.Map<List<ReviewerDto>>(_reviewerRepository.GetAll());
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(reviewers);
    } 
    
    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = typeof(Reviewer))]
    [ProducesResponseType(400)]
    public IActionResult GetReviewerById(int id)
    {
        if (!_reviewerRepository.Exists(id))
            return NotFound();
        var reviewer = _mapper.Map<OwnerDto>(_reviewerRepository.GetById(id));
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(reviewer);
    }
    
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateReviewer([FromBody] ReviewerDto reviewerDtoCreate)
    {
        if (reviewerDtoCreate == null)
            return BadRequest(ModelState);

        var reviewer = _reviewerRepository.GetAll()
            .Where(c => c.LastName.Trim().ToUpper() == reviewerDtoCreate.LastName.Trim().ToUpper()).FirstOrDefault();

        if (reviewer != null)
        {
            ModelState.AddModelError("", "Reviewer already exists");
            return StatusCode(422, ModelState);
        }
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var reviewerMap = _mapper.Map<Reviewer>(reviewerDtoCreate);

        if (!_reviewerRepository.Create(reviewerMap))
        {
            ModelState.AddModelError("", "Something went wrong while savin");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }

    [HttpPut("{reviewerId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult UpdateReviewer(int reviewerId, [FromBody] ReviewDto reviewerUpdate)
    {
        if (reviewerUpdate == null)
            return BadRequest(ModelState);

        if (reviewerId != reviewerUpdate.Id)
            return BadRequest(ModelState);

        if (!_reviewerRepository.Exists(reviewerId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        var reviewerMap = _mapper.Map<Reviewer>(reviewerUpdate);

        if (!_reviewerRepository.Update(reviewerMap))
        {
            ModelState.AddModelError("", "Something went wrong updating category");
            return StatusCode(500, ModelState);
        }
        
        return NoContent();
    }

    [HttpDelete("{reviewerId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult DeleteReviewer(int reviewerId)
    {
        if (!_reviewerRepository.Exists(reviewerId))
            return NotFound();

        var reviewerToDelete = _reviewerRepository.GetById(reviewerId);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!_reviewerRepository.Delete(reviewerToDelete))
        {
            ModelState.AddModelError("", "Something went wrong deleting category");
        }
        
        return NoContent();
    }
}