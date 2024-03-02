using Microsoft.AspNetCore.Mvc;
using TmpApi.Services;

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

        foreach (var item in result.Errors)
        {
            ModelState.AddModelError(item.Key, item.Value);
        }

        if (!result.Success)
        {
            return ValidationProblem(ModelState);
        }

        return Ok(result.Item);
    }
}
