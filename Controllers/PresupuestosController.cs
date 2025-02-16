using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

public class PresupuestosController : Controller
{
    private readonly ILogger<PresupuestosController> _logger;
    private PresupuestosRepository _repoPresupuestos;
    private ClientesRepository _repoClientes;
    private ProductoRepository _repoProductos ;
    public PresupuestosController(ILogger<PresupuestosController> logger)
    {
        _logger = logger;
        _repoPresupuestos= new PresupuestosRepository();
        _repoClientes= new ClientesRepository();
        _repoProductos= new ProductoRepository();
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
        var clientes = _repoClientes.ListarClientes();
        var producto = _repoProductos.ListarProdcutos();
        var clientesVM= clientes.Select(c=> new ClienteViewModel(c.ClienteId, c.Nombre)).ToList();
        var productoVM= producto.Select(p=> new ProductoViewModel(p.IdProducto, p.Descripcion)).ToList();
        var viewmodel=  new PresupuestoAltaViewModel(clientesVM,productoVM);
        return View(viewmodel);
    }
    [HttpPost]
    public IActionResult CrearPresupuesto(PresupuestoAltaViewModel presupuestoVM) // va a dejar de recibir un presupuesto y va a recibir un viewmodel
    {
        if(!ModelState.IsValid) return RedirectToAction ("Index");
        var cliente= _repoClientes.ObtenerClientePorId(presupuestoVM.IdClienteSeleccionado);
        var prod= _repoProductos.ObtenerProductoPorId(presupuestoVM.IdProductoSeleccionado);
        var listaProd= new List<PresupuestosDetalle>
        {
            new PresupuestosDetalle(prod, presupuestoVM.Cantidad)
        };
        var presupuesto= new Presupuestos(0,cliente, listaProd, presupuestoVM.Fecha);
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
        var clientes=_repoClientes.ListarClientes();
        var ClientesVM= clientes.Select(c=> new ClienteViewModel(c.ClienteId,c.Nombre)).ToList();
        var viewmodel= new ModificarPresupuestoViewModel(idPres, ClientesVM, presupuesto.FechaCreacion, presupuesto.Cliente.ClienteId);

        
        return View(viewmodel);
    }
    [HttpPost]
    public IActionResult Modificar(ModificarPresupuestoViewModel viewModel)
    {
        if(!ModelState.IsValid) return RedirectToAction ("Index");
        var cliente= _repoClientes.ObtenerClientePorId(viewModel.IdClienteSeleccionado);
        var detalle= _repoPresupuestos.ObtenerPresupuestoPorId(viewModel.IdPresupuesto);
        var presupuesto= new Presupuestos(0,cliente,detalle.Detalle,viewModel.Fecha);
        _repoPresupuestos.ModificarPresupuesto(presupuesto, viewModel.IdPresupuesto);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult ModificarDetalleForm(int idProd, int idPres)
    {
        //var presupuesto=_repoPresupuestos.ObtenerPresupuestoPorId(idPres);
        //var detalle=presupuesto.Detalle.Find(d=>d.Producto.IdProducto==idProd);
        //ViewData["idPres"]=idPres; // eniva a la vista la informacion en @ViewData["idPres"]
        //return View(detalle);     lo guarde para ver el uso del viewdata
        var presupuesto=_repoPresupuestos.ObtenerPresupuestoPorId(idPres);
        var listaProductos= _repoProductos.ListarProdcutos();
        var listadoProductosVM= listaProductos.Where(w=> !presupuesto.Detalle.Any(a=> a.Producto.IdProducto==w.IdProducto)).Select(s=> new ProductoViewModel(s.IdProducto,s.Descripcion)).ToList(); // listo todos los productos que no esten en el presupuesto
        var detalle=presupuesto.Detalle.Find(d=>d.Producto.IdProducto==idProd);
        listadoProductosVM.Add(new ProductoViewModel(detalle.Producto.IdProducto, detalle.Producto.Descripcion)); // como al crear la lista ignore todos los productos que el presupuesto tenia, incluyo ahora el que estaba seleccionado (por si solo se quisiera modificar la cantidad, sin esto no se podria ya que me obligaria tambien a cambiar de producto)
        var viewmodel= new ModificarDetalleViewModel(listadoProductosVM, detalle.Cantidad, detalle.Producto.IdProducto, idPres, idProd);
        return View(viewmodel);  
    }
    // public IActionResult ModificarDetalle(PresupuestosDetalle detalle, int idPres, int idProdViejo)
    // {
    //     _repoPresupuestos.ModificarDetalle(detalle,idPres, idProdViejo); guardado para ver el uso del viewdata que llega del forumalario a traves del input hidden con el name coincidente con el parametro
    //     return RedirectToAction("VerDetalle");
    // }
    public IActionResult ModificarDetalle (ModificarDetalleViewModel viewmodel)
    {
        if(!ModelState.IsValid) return RedirectToAction("VerDetalle", new {id= viewmodel.IdPresupuesto});
        var presupuesto= _repoPresupuestos.ObtenerPresupuestoPorId(viewmodel.IdPresupuesto);
        var detalle= presupuesto.Detalle.Find(f=> f.Producto.IdProducto==viewmodel.IdProdAnterior);
        detalle.Producto= _repoProductos.ObtenerProductoPorId(viewmodel.IdProductoSeleccionado);
        detalle.Cantidad=viewmodel.Cantidad;
        _repoPresupuestos.ModificarDetalle(detalle,viewmodel.IdPresupuesto,viewmodel.IdProdAnterior);
        return RedirectToAction("VerDetalle", new { id=viewmodel.IdPresupuesto});
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
    var presu=_repoPresupuestos.ObtenerPresupuestoPorId(id);
    var listadoProductos= _repoProductos.ListarProdcutos();
    var listadoProductosVM= listadoProductos.Where(p=> !presu.Detalle.Any(a=> a.Producto.IdProducto==p.IdProducto)).Select(s=> new ProductoViewModel(s.IdProducto, s.Descripcion)).ToList(); // where para filtrar: donde en presu.detalle no haya ningun a.prod.id (a del any) igual al p.idprod (p del where) si no se cumple la igualdad selecciono el s.nombre y s.id (s del select) creando un objeto listadoproductoaltaviewmodel y agregandolo a la lista  
    var viewmodel= new AgregarProductoAlPresupuestoViewModel(id, listadoProductosVM);
    return View(viewmodel);
   }
   [HttpPost]
   public IActionResult AgregarProductoAlPresupuesto(AgregarProductoAlPresupuestoViewModel viewmodel)
   {
    if(!ModelState.IsValid)return RedirectToAction("VerDetalle", new {id= viewmodel.IdPresupuesto});
    _repoPresupuestos.AgregarProductoyCantidad(viewmodel.IdPresupuesto,viewmodel.IdProductoSeleccionado, viewmodel.Cantidad);
    return RedirectToAction("Index");
   }
}