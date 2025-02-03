using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

public class PresupuestosController : Controller
{
    private readonly ILogger<PresupuestosController> _logger;
    private PresupuestosRepository _repoPresupuestos;
    public PresupuestosController(ILogger<PresupuestosController> logger)
    {
        _logger = logger;
        _repoPresupuestos= new PresupuestosRepository();

    }
    [HttpGet]
    public IActionResult Index()
    {
        var lista=_repoPresupuestos.ListarPresupuestos();
        return View(lista);
    }
    [HttpGet]
    public IActionResult CrearPresupuesto()
    {
        var repoClientes= new ClientesRepository();
        var repoProducto= new ProductoRepository();
        var clientes = repoClientes.ListarClientes();
        var producto = repoProducto.ListarProdcutos();
        var clientesVM= clientes.Select(c=> new ClienteViewModel(c.ClienteId, c.Nombre)).ToList();
        var productoVM= producto.Select(p=> new ProductoViewModel(p.IdProducto, p.Descripcion)).ToList();
        var viewmodel=  new PresupuestoAltaViewModel(clientesVM,productoVM);
        return View(viewmodel);
    }
    [HttpPost]
    public IActionResult CrearPresupuesto(Presupuestos presupuesto) // va a dejar de recibir un presupuesto y va a recibir un viewmodel
    {
        // el controlador se encargara de transformar el viewmodel en el item presupuesto para que lo procese el repositorio (esto sera asi en todos los casos)
        _repoPresupuestos.CrearPresupuesto(presupuesto); 
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult VerDetalle(int id)
    {
        var presupuesto=_repoPresupuestos.ObtenerPresupuestoPorId(id);
        return View(presupuesto);
    }
    [HttpGet]
    public IActionResult ModificarForm(int idPres)
    {   
        var presupuesto= _repoPresupuestos.ObtenerPresupuestoPorId(idPres); 
        return View(presupuesto);
    }
    [HttpPost]
    public IActionResult Modificar(Presupuestos presupuesto)
    {
        _repoPresupuestos.ModificarPresupuesto(presupuesto,presupuesto.IdPresupuesto);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult ModificarDetalleForm(int idProd, int idPres)
    {
        var presupuesto=_repoPresupuestos.ObtenerPresupuestoPorId(idPres);
        var detalle=presupuesto.Detalle.Find(d=>d.Producto.IdProducto==idProd);
        ViewData["idPres"]=idPres; // eniva a la vista la informacion en @ViewData["idPres"]
        return View(detalle);     
    }
    public IActionResult ModificarDetalle(PresupuestosDetalle detalle, int idPres, int idProdViejo)
    {
        _repoPresupuestos.ModificarDetalle(detalle,idPres, idProdViejo);
        return RedirectToAction("VerDetalle");
    }
    [HttpGet]
    public IActionResult EliminarPresupuesto(int id)
    {
        return View(id);
    }
    [HttpGet] //get porque mostramos la lista del index
   public IActionResult ConfirmarEliminarPresupuesto(int id)
   {
        _repoPresupuestos.EliminarPresupuesto(id);
        return RedirectToAction("Index");
   }
   [HttpGet]
   public IActionResult AgregarProductoAlPresupuesto(int id)
   {
    return View(id);
   }
   [HttpPost]
   public IActionResult AgregarProductoAlPresupuesto(int idPres, int cant, int idProd)
   {
    _repoPresupuestos.AgregarProductoyCantidad(idPres,idProd,cant);
    return RedirectToAction("Index");
   }
}