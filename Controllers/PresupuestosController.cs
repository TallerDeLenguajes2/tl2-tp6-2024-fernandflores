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
    public IActionResult CrearPresupuesto(PresupuestoAltaViewModel presupuestoVM, DateTime FechaCreacion, int cantidad ) // va a dejar de recibir un presupuesto y va a recibir un viewmodel
    {
        var _repoClientes= new ClientesRepository();
        var _repoProd= new ProductoRepository();
        var cliente= _repoClientes.ObtenerClientePorId(presupuestoVM.IdClienteSeleccionado);
        var prod= _repoProd.ObtenerProductoPorId(presupuestoVM.IdProductoSeleccionado);
        var listaProd= new List<PresupuestosDetalle>
        {
            new PresupuestosDetalle(prod, cantidad)
        };
        var presupuesto= new Presupuestos(0,cliente, listaProd, FechaCreacion);
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
    var _repoProductos= new ProductoRepository();
    var presu=_repoPresupuestos.ObtenerPresupuestoPorId(id);
    var listadoProductos= _repoProductos.ListarProdcutos();
    var listadoProductosVM= listadoProductos.Where(p=> !presu.Detalle.Any(a=> a.Producto.IdProducto==p.IdProducto)).Select(s=> new ListadoProductosAltaPresupuestoViewModel(s.IdProducto, s.Descripcion)).ToList(); // where para filtrar: donde en presu.detalle no haya ningun a.prod.id (a del any) igual al p.idprod (p del where) si no se cumple la igualdad selecciono el s.nombre y s.id (s del select) creando un objeto listadoproductoaltaviewmodel y agregandolo a la lista  
    var viewmodel= new ProductoAltaPresupuestoViewModel(id, listadoProductosVM);
    return View(viewmodel);
   }
   [HttpPost]
   public IActionResult AgregarProductoAlPresupuesto(ProductoAltaPresupuestoViewModel viewmodel, int cantidad)
   {

    _repoPresupuestos.AgregarProductoyCantidad(viewmodel.IdPresupuesto,viewmodel.IdProductoSeleccionado, cantidad);
    return RedirectToAction("Index");
   }
}