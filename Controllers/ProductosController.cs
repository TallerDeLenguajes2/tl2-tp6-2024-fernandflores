using Microsoft.AspNetCore.Mvc;

public class ProductosController : Controller 
{
    private readonly ILogger<ProductosController> _logger;
    private IProductoRepository _repositorioProducto;

    public ProductosController(ILogger<ProductosController> logger, IProductoRepository ProductoRepository)
    {
        _logger = logger;
        _repositorioProducto= ProductoRepository;
    }
    [HttpGet]
    public IActionResult Index()
    {
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login"); // si no hay nadie logeado, lo manda al logearse
        ViewBag.Rol=HttpContext.Session.GetString("rol"); // enviamos en un viewbag el rol a la vista (viewbag sirve para enviar datos del controlador a la misma vista, no para redirecttoactions)
        return View(_repositorioProducto.ListarProdcutos());
    }
    [HttpGet]
    public IActionResult CrearProducto () 
    {
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");// si no hay nadie logeado, lo manda al logearse
        if(HttpContext.Session.GetString("rol")!="admin") //revisa si es admin
        {
            TempData["RolError"]="no tenes los privilegios para hacer esta accion";
            return RedirectToAction("Index"); // como es redirectoaction un viewbag no serivria, asi que lo mandamos con tempdata
        }
        return View(); // me lleva a la vista de crearProducto (el formulario)
    }
    [HttpPost]
    public IActionResult CrearProducto(Productos producto)
    {
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");// si no hay nadie logeado, lo manda al logearse
        if(HttpContext.Session.GetString("rol")!="admin") //revisa si es admin
        {
            TempData["RolError"]="no tenes los privilegios para hacer esta accion";
            return RedirectToAction("Index"); // como es redirectoaction un viewbag no serivria, asi que lo mandamos con tempdata
        }
        _repositorioProducto.CrearProducto(producto);  // actua en el repositorio
        return  RedirectToAction("Index"); // redirige a la vista de index
    }
    [HttpGet] // obtengo el producto a modificar es get porque es donde muestra/pide los datos en el formulario
    public IActionResult ModificarProducto (int id)
    {
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");// si no hay nadie logeado, lo manda al logearse
        if(HttpContext.Session.GetString("rol")!="admin") //revisa si es admin
        {
            TempData["RolError"]="no tenes los privilegios para hacer esta accion";
            return RedirectToAction("Index"); // como es redirectoaction un viewbag no serivria, asi que lo mandamos con tempdata
        }
        var producto= _repositorioProducto.ObtenerProductoPorId(id);
        if(producto==null) return NotFound("no existe el producto");
        return View(producto);
    }
    [HttpPost] // es post porque aqui es donde el back modifica
    public IActionResult ConfirmarModificarProducto (Productos producto)
    {
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");// si no hay nadie logeado, lo manda al logearse
        if(HttpContext.Session.GetString("rol")!="admin") //revisa si es admin
        {
            TempData["RolError"]="no tenes los privilegios para hacer esta accion";
            return RedirectToAction("Index"); // como es redirectoaction un viewbag no serivria, asi que lo mandamos con tempdata
        }
        _repositorioProducto.ModificarProducto(producto.IdProducto, producto);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult EliminarProducto(int id)
    {
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");// si no hay nadie logeado, lo manda al logearse
        if(HttpContext.Session.GetString("rol")!="admin") //revisa si es admin
        {
            TempData["RolError"]="no tenes los privilegios para hacer esta accion";
            return RedirectToAction("Index"); // como es redirectoaction un viewbag no serivria, asi que lo mandamos con tempdata
        }
        return View(id);
    }
    [HttpGet] //get porque mostramos la lista del index
   public IActionResult ConfirmarEliminarProducto(int id)
   {
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"))) return RedirectToAction("Index", "Login");// si no hay nadie logeado, lo manda al logearse
        if(HttpContext.Session.GetString("rol")!="admin") //revisa si es admin
        {
            TempData["RolError"]="no tenes los privilegios para hacer esta accion";
            return RedirectToAction("Index"); // como es redirectoaction un viewbag no serivria, asi que lo mandamos con tempdata
        }
        if(_repositorioProducto.ObtenerProductoPorId(id)==null) return NotFound();
        _repositorioProducto.EliminarPorId(id);
        return RedirectToAction("Index");
   }
}