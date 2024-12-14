using Microsoft.AspNetCore.Mvc.Routing;

namespace SurveyBasket.Api.Controllers;

[Route("api/[controller]")] // /api/polls
[ApiController]
public class PollsController(IPollService pollService) : ControllerBase
{
    private readonly IPollService _pollService = pollService;
    //add actions / endpoint
    [HttpGet("")] // verb
    // IActionResults allows me to return whatever i need, data or status code
    public IActionResult GetAll()
    {
        var polls = _pollService.GetAll();
        polls.Adapt<IEnumerable<Poll>, PollResponse>();
        return Ok(polls);
    }
    [HttpGet("{Id}")]
    public IActionResult Get([FromRoute] int Id)
    {
        var poll = _pollService.Get(Id);
        var response = poll.Adapt<PollResponse>();
        return response is null ? NotFound() : Ok(response);
    }

    [HttpPost("")]
    public IActionResult Add([FromBody] PollRequest request)
    {
        var newPoll = _pollService.Add(request.Adapt<Poll>());
        
        return CreatedAtAction(nameof(Get) , new { Id = newPoll.Id}, newPoll);
    }
    [HttpPut("{Id}")]
    public IActionResult Update([FromRoute]int Id, [FromBody] PollRequest request)
    { 
       var isUpdated = _pollService.Update(Id, request.Adapt<Poll>()); 
        if(!isUpdated) return NotFound();
        return NoContent();
    }
    [HttpDelete("{Id}")]
    public IActionResult Delete([FromRoute] int id)
    {
        var isDeleted = _pollService.Delete(id);
        if (!isDeleted) return NotFound();
        return NoContent();
    }
}
