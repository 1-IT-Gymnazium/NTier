using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TmpApi.Services;
using TmpApi.Utilities;

namespace TmpApi.Controllers;
[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHomeService _homeService;

    public HomeController(ILogger<HomeController> logger, IHomeService homeService)
    {
        _logger = logger;
        _homeService = homeService;
    }

    [HttpPost]
    public async Task<ActionResult<HomeDetail>> Create([FromBody] HomeCreate model)
    {
        var result = await _homeService.Create(model);
        
        ModelState.AddAllErrors(result);

        if (!result.Success)
        {
            return ValidationProblem(ModelState);
        }

        return Ok(result.Item);
    }
}
