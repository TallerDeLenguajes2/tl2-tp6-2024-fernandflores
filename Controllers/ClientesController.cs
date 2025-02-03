using Microsoft.AspNetCore.Mvc;

public class ClientesController : Controller 
{
     private readonly ILogger<ClientesController> _logger;
    private ClientesRepository _repoClientes;
    public ClientesController(ILogger<ClientesController> logger)
    {
        _logger = logger;
        _repoClientes= new ClientesRepository();
    }
    [HttpGet]
    public IActionResult Index()
    {
        var lista= _repoClientes.ListarClientes();
        return View(lista);
    }
    [HttpGet]
    public IActionResult CrearCliente()
    {
        return View();
    }
    [HttpPost]
    public IActionResult CrearCliente(Clientes cliente)
    {
        _repoClientes.CrearCliente(cliente);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult ModificarClienteForm (int id)
    {
        var cliente= _repoClientes.ObtenerClientePorId(id);
        return View(cliente);
    }
    [HttpPost]
    public IActionResult ModificarCliente (Clientes cliente)
    {
        _repoClientes.ModificarCliente(cliente);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult EliminarCliente (int id)
    {
        return View(id);
    }
    [HttpGet]
    public IActionResult ConfirmarEliminarCliente (int id)
    {
        _repoClientes.EliminarCliente(id);
        return RedirectToAction("Index");
    }
}