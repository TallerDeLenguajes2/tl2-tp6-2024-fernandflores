using Microsoft.AspNetCore.Mvc;

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
        return View();
    }
    [HttpPost]
    public IActionResult CrearPresupuesto(Presupuestos presupuesto)
    {
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