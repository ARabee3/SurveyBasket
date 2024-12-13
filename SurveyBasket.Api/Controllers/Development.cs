using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Api.Services;

namespace SurveyBasket.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class Development : ControllerBase
{
    private readonly ILogger _logger;
    public Development
        (ILogger<Development> logger)
    {
        _logger = logger;
    }


    // Dependency injection using constructor
    
    [HttpGet]
    public IActionResult Run(
        [FromKeyedServices("windows")] IOperationTransient windowsService, 
        [FromKeyedServices("macos")] IOperationTransient macService
        )
    {
        //var os = new WindowsOsService(); // against Depndency Inversion, becuase the high level module (Development Controller) depands directly on Low Level module (WindowsOs Class)
       // _logger.LogWarning("Windows {0}",windowsService.OperationId);
        _logger.LogError("MacOs {0}",macService.OperationId);
        return Ok();
    }
}
