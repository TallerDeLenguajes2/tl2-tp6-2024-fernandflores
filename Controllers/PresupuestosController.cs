using Microsoft.AspNetCore.Mvc;

public class PresupuestosController : Controller
{
    private readonly ILogger<PresupuestosController> _logger;
    public PresupuestosController(ILogger<PresupuestosController> logger)
    {
        _logger = logger;
    }
    
}