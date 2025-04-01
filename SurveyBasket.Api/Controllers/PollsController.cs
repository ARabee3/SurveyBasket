using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Routing;
using SurveyBasket.Api.Abstractions;
using SurveyBasket.Api.Contracts.Poll;
using System.Threading;

namespace SurveyBasket.Api.Controllers;

[Route("api/[controller]")] // /api/polls
[ApiController]
[Authorize]
public class PollsController(IPollService pollService) : ControllerBase
{

    private readonly IPollService _pollService = pollService;
    //add actions / endpoint
    [HttpGet("")] // verb
    // IActionResults allows me to return whatever i need, data or status code
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var polls = await _pollService.GetAllAsync(cancellationToken);
        return Ok(polls);
    } 

    [HttpGet("current")] // verb
    public async Task<IActionResult> GetCurrent(CancellationToken cancellationToken)
    {
        var polls = await _pollService.GetCurrentAsync(cancellationToken);
        return Ok(polls);
    }
    [HttpGet("{Id}")]
    public async Task<IActionResult> Get([FromRoute] int Id, CancellationToken cancellationToken)
    {
        var result = await _pollService.GetAsync(Id, cancellationToken);
        return result.IsSuccess
           ? Ok(result.Value)
           : Problem(statusCode: StatusCodes.Status404NotFound, title: result.Error.Code, detail: result.Error.Description);
    }

    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] PollRequest request, CancellationToken cancellationToken)
    {
        var newPoll = await _pollService.AddAsync(request, cancellationToken);
        return CreatedAtAction(nameof(Get), new {id= newPoll.id},newPoll);
    }
    [HttpPut("{Id}")]
    public async Task<IActionResult> Update([FromRoute] int Id, [FromBody] PollRequest request, CancellationToken cancellationToken)
    {
        var isUpdated = await _pollService.UpdateAsync(Id, request , cancellationToken);
        return isUpdated.IsSuccess ? NoContent() : NotFound(isUpdated.Error);
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var isDeleted = await _pollService.DeleteAsync(id, cancellationToken);
        return isDeleted.IsSuccess ? NoContent() : Problem(statusCode: StatusCodes.Status404NotFound, title: isDeleted.Error.Code, detail: isDeleted.Error.Description);
    }

    [HttpPut("{Id}/togglePublish")]
    public async Task<IActionResult> TogglePublish([FromRoute] int id, CancellationToken cancellationToken)
    {
        var status = await _pollService.TogglePublishStatusAsync(id, cancellationToken);
        return status.IsSuccess ? NoContent() : Problem(statusCode: StatusCodes.Status404NotFound, title: status.Error.Code, detail: status.Error.Description);
    }
}
