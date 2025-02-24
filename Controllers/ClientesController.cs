using Microsoft.AspNetCore.Mvc;

public class ClientesController : Controller 
{
    private readonly ILogger<ClientesController> _logger;
    private IClienteRepository _repoClientes;
    public ClientesController(ILogger<ClientesController> logger, IClienteRepository ClienteRepository)
    {
        _logger = logger;
        _repoClientes= ClienteRepository;
    }
    [HttpGet]
    //en todos los index crear el viewbag con el rol
    //todos los endpoint que requieran ser admin, si un no-admin entra a traves de la url al endpoint, se manda el error en tempdata el cual se muestra en el index que se redirecciona en ese endpoint (lo comente un poco en crearCliente) 
    public IActionResult Index()
    {
        try
        {
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login"); // si no hay nadie logeado, lo manda al logearse
            ViewBag.Rol=HttpContext.Session.GetString("rol"); // enviamos en un viewbag el rol a la vista (viewbag sirve para enviar datos del controlador a la misma vista, no para redirecttoactions)
            var lista= _repoClientes.ListarClientes();
            return View(lista);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
            TempData["MensajeCatch"]="no se pudo cargar la pagina index de clientes";
           // ViewBag.MensajeCatch= "no se pudo cargar la pagina index de clientes";  no lo usamos porque es redirecttoaction, entonces perdera la informacion (el viewbag no funciona en los redirecttoaction)
            return RedirectToAction("Index");
        }
    }
    [HttpGet]
    public IActionResult CrearCliente()
    {
        try
        {
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login"); // revisar si esta logeado
            if(HttpContext.Session.GetString("rol")!="admin")// si no es admin lo mandamos al index con un mensaje de error
            {
                TempData["RolError"]="no tenes los privilegios para hacer esta accion"; //se envia a la vista index este error
                return RedirectToAction("Index"); // como es redirectoaction un viewbag no serivria, asi que lo mandamos con tempdata
            }
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            TempData["MensajeCatch"]= "no se pudo cargar la vista de crear el cliente";
            return RedirectToAction ("Index");
        }
    }
    [HttpPost]
    public IActionResult CrearCliente(Clientes cliente)
    {
        try
        {
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");
            if(HttpContext.Session.GetString("rol")!="admin") // si no es admin lo mandamos al index con un mensaje de error
            {
                TempData["RolError"]="no tenes los privilegios para hacer esta accion";
                return RedirectToAction("Index"); // como es redirectoaction un viewbag no serivria, asi que lo mandamos con tempdata
            }
            _repoClientes.CrearCliente(cliente);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            TempData["MensajeCatch"]= "no se pudo crear el cliente";
            return RedirectToAction ("Index");
        }
    }
    [HttpGet]
    public IActionResult ModificarClienteForm (int id)
    {
        try
        {
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login"); // revisar si esta logeado
            if(HttpContext.Session.GetString("rol")!="admin") //revisa si es admin
            {
                TempData["RolError"]="no tenes los privilegios para hacer esta accion";
                return RedirectToAction("Index"); // como es redirectoaction un viewbag no serivria, asi que lo mandamos con tempdata
            }
            var cliente= _repoClientes.ObtenerClientePorId(id);
            return View(cliente);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            TempData["MensajeCatch"]= "no se pudo cargar el formulario";
            return RedirectToAction ("Index");
        }
    }
    [HttpPost]
    public IActionResult ModificarCliente (Clientes cliente)
    {
        try
        {
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login"); // revisar si esta logeado
            if(HttpContext.Session.GetString("rol")!="admin") //revisa si es admin
            {
                TempData["RolError"]="no tenes los privilegios para hacer esta accion";
                return RedirectToAction("Index"); // como es redirectoaction un viewbag no serivria, asi que lo mandamos con tempdata
            }
            _repoClientes.ModificarCliente(cliente);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            TempData["MensajeCatch"]= "no se pudo modificar el cliente" ;
            return RedirectToAction ("Index");
        }
    }
    [HttpGet]
    public IActionResult EliminarCliente (int id)
    {
        try
        {
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login"); // revisar si esta logeado
            if(HttpContext.Session.GetString("rol")!="admin")
            {
                TempData["RolError"]="no tenes los privilegios para hacer esta accion";
                return RedirectToAction("Index"); // como es redirectoaction un viewbag no serivria, asi que lo mandamos con tempdata
            }
            return View(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            TempData["MensajeCatch"]= "no se pudo cargar la vista de eliminar cliente" ;
            return RedirectToAction ("Index");
        }
    }
    [HttpGet]
    public IActionResult ConfirmarEliminarCliente (int id)
    {
        try
        {    
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login"); // revisar si esta logeado
            if(HttpContext.Session.GetString("rol")!="admin")
            {
                TempData["RolError"]="no tenes los privilegios para hacer esta accion";
                return RedirectToAction("Index"); // como es redirectoaction un viewbag no serivria, asi que lo mandamos con tempdata
            }
            _repoClientes.EliminarCliente(id);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            TempData["MensajeCatch"]= "no se pudo eliminar el cliente";
            return RedirectToAction ("Index");
        }
    }
}