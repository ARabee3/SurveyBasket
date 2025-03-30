using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Routing;
using SurveyBasket.Api.Contracts.Poll;
using System.Threading;

namespace SurveyBasket.Api.Controllers;

[Route("api/[controller]")] // /api/polls
[ApiController]
public class PollsController(IPollService pollService) : ControllerBase
{

    private readonly IPollService _pollService = pollService;
    //add actions / endpoint
    [HttpGet("")] // verb
    // IActionResults allows me to return whatever i need, data or status code
    [Authorize]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var polls = await _pollService.GetAllAsync(cancellationToken);
        polls.Adapt<IEnumerable<Poll>, PollResponse>();
        return Ok(polls);
    }
    [HttpGet("{Id}")]
    public async Task<IActionResult> Get([FromRoute] int Id, CancellationToken cancellationToken)
    {
        var poll = await _pollService.GetAsync(Id, cancellationToken);
        var response = poll.Adapt<PollResponse>();
        return  response is null ? NotFound() : Ok(response);
    }

    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] PollRequest request, CancellationToken cancellationToken)
    {
        var newPoll = await _pollService.AddAsync(request.Adapt<Poll>(), cancellationToken);

        return CreatedAtAction(nameof(Get), new { Id = newPoll.Id }, newPoll);
    }
    [HttpPut("{Id}")]
    public async Task<IActionResult> Update([FromRoute] int Id, [FromBody] PollRequest request, CancellationToken cancellationToken)
    {
        var isUpdated = await  _pollService.UpdateAsync(Id, request.Adapt<Poll>(), cancellationToken);
        if (!isUpdated) return NotFound();
        return NoContent();
    }
    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete([FromRoute] int id,CancellationToken cancellationToken)
    {
        var isDeleted = await _pollService.DeleteAsync(id, cancellationToken);
        if (!isDeleted) return NotFound();
        return NoContent();
    }
    [HttpPut("{Id}/togglePublish")]
    public async Task<IActionResult> TogglePublish([FromRoute] int id,CancellationToken cancellationToken)
    {
        var status = await _pollService.TogglePublishStatusAsync(id, cancellationToken);
        if (!status) return NotFound();
        return NoContent();
    }
}
