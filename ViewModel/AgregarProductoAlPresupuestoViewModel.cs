using System.ComponentModel.DataAnnotations;
public class AgregarProductoAlPresupuestoViewModel
{
    private int _idPresupuesto;
    private List<ProductoViewModel> _productos;
    private int _idProductoSeleccionado;
    private int _cantidad;

    public AgregarProductoAlPresupuestoViewModel()
    {
        _productos= new List<ProductoViewModel>();
    }

    public AgregarProductoAlPresupuestoViewModel(int id, List<ProductoViewModel> lista)
    {
        _idPresupuesto= id;
        _productos= lista;
    }

    public int IdPresupuesto { get => _idPresupuesto; set => _idPresupuesto = value; }
    public List<ProductoViewModel> Productos { get => _productos; set => _productos = value; }
    [Required (ErrorMessage="debe seleccionar un producto")]
    public int IdProductoSeleccionado { get => _idProductoSeleccionado; set => _idProductoSeleccionado = value; }
    [Required][Range(1,50, ErrorMessage ="maximo 50 unidades por producto")]
    public int Cantidad { get => _cantidad; set => _cantidad = value; }
}