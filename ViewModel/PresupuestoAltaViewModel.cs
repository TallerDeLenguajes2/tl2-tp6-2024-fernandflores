using System.ComponentModel.DataAnnotations;
public class PresupuestoAltaViewModel
{

    private List<ClienteViewModel> _clientes;
    private List<ProductoViewModel> _productos;
    private int _idClienteSeleccionado;
    private int _idProductoSeleccionado;
    private DateTime _fecha;
    private int _cantidad;
    public PresupuestoAltaViewModel()
    {
        _clientes= new List<ClienteViewModel>();
        _productos= new List<ProductoViewModel>();
    }

    public PresupuestoAltaViewModel(List<ClienteViewModel> cltes, List<ProductoViewModel> prodtos)
    {
        _clientes=cltes;
        _productos= prodtos;
        _fecha= DateTime.Today;
      
    }

    public List<ProductoViewModel> Productos { get => _productos; set => _productos = value; }
    public List<ClienteViewModel> Clientes { get => _clientes; set => _clientes = value; } 
    [Required (ErrorMessage="debe seleccionar un cliente")]
    public int IdClienteSeleccionado { get => _idClienteSeleccionado; set => _idClienteSeleccionado = value; }
    [Required (ErrorMessage="debe seleccionar un producto")]
    public int IdProductoSeleccionado { get => _idProductoSeleccionado; set => _idProductoSeleccionado = value; }
    [Required (ErrorMessage ="debe seleccionar una fecha")]
    public DateTime Fecha { get => _fecha; set => _fecha = value; }
    [Required][Range(1, 50, ErrorMessage ="debe ser un numero entre 1 y 50")]
    public int Cantidad { get => _cantidad; set => _cantidad = value; }
}
