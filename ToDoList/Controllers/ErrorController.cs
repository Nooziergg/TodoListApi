using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Services.Interfaces;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : ControllerBase
{
    private readonly IErrorService _errorService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ErrorController(IErrorService errorService, IWebHostEnvironment webHostEnvironment)
    {
        _errorService = errorService;
        _webHostEnvironment = webHostEnvironment;
    }

    [Route("/error")]
    public IActionResult HandleError()
    {
        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
        var exception = context?.Error;

        var isDev = _webHostEnvironment.EnvironmentName == "Development";
        var error = _errorService.HandleError(exception, isDev);

        return StatusCode(error.StatusCode, error);
    }
}
