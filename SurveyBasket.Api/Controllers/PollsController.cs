namespace SurveyBasket.Api.Controllers;

[Route("api/[controller]")] // /api/polls
[ApiController]
public class PollsController(IPollService pollService) : ControllerBase
{
    private readonly IPollService _pollService = pollService;
    //add actions / endpoint
    [HttpGet("")] // verb
    // IActionResults allows me to return whatever i need, data or status code
    public IActionResult GetAll() => Ok(_pollService.GetAll());

    [HttpGet("{Id}")]
    public IActionResult Get(int Id) 
    {
        var poll = _pollService.Get(Id);
        return poll is null ? NotFound() : Ok(poll);
    }

    [HttpPost("")]
    public IActionResult Add(Poll poll)
    {
        var newPoll = _pollService.Add(poll);
        return CreatedAtAction(nameof(Get) , new { Id = poll.Id}, poll);
    }
    [HttpPut("{Id}")]
    public IActionResult Update(int Id, Poll poll)
    { 
       var isUpdated = _pollService.Update(Id, poll); 
        if(!isUpdated) return NotFound();
        return NoContent();
    }
    [HttpDelete("{Id}")]
    public IActionResult Delete(int id)
    {
        var isDeleted = _pollService.Delete(id);
        if (!isDeleted) return NotFound();
        return NoContent();
    }
}
