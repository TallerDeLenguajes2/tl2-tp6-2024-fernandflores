using Microsoft.AspNetCore.Mvc;

public class ProductosController : Controller 
{
    private readonly ILogger<ProductosController> _logger;
    private ProductoRepository _repositorioProducto;

    public ProductosController(ILogger<ProductosController> logger)
    {
        _logger = logger;
        _repositorioProducto= new ProductoRepository();
    }
    [HttpGet]
    public IActionResult Index()
    {
        return View(_repositorioProducto.ListarProdcutos());
    }
    [HttpGet]
    public IActionResult CrearProducto () 
    {
        return View(); // me lleva a la vista de crearProducto (el formulario)
    }
    [HttpPost]
    public IActionResult CrearProducto(Productos producto)
    {
        _repositorioProducto.CrearProducto(producto);  // actua en el repositorio
        return  RedirectToAction("Index"); // redirige a la vista de index
    }
    [HttpGet] // obtengo el producto a modificar es get porque es donde muestra/pide los datos en el formulario
    public IActionResult ModificarProducto (int id)
    {
        var producto= _repositorioProducto.ObtenerProductoPorId(id);
        if(producto==null) return NotFound("no existe el producto");
        return View(producto);
    }
    [HttpPost] // es post porque aqui es donde el back modifica
    public IActionResult ConfirmarModificarProducto (Productos producto)
    {
        _repositorioProducto.ModificarProducto(producto.IdProducto, producto);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult EliminarProducto(int id)
    {
        return View(id);
    }
    [HttpGet] //get porque mostramos la lista del index
   public IActionResult ConfirmarEliminarProducto(int id)
   {
        if(_repositorioProducto.ObtenerProductoPorId(id)==null) return NotFound();
        _repositorioProducto.EliminarPorId(id);
        return RedirectToAction("Index");
   }
}