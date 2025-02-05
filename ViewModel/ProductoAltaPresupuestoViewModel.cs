public class ProductoAltaPresupuestoViewModel
{
    private int _idPresupuesto;
    private List<ListadoProductosAltaPresupuestoViewModel> _productos;
    private int _idProductoSeleccionado;

    public ProductoAltaPresupuestoViewModel()
    {
    }

    public ProductoAltaPresupuestoViewModel(int id, List<ListadoProductosAltaPresupuestoViewModel> lista)
    {
        _idPresupuesto= id;
        _productos= lista;
    }

    public int IdPresupuesto { get => _idPresupuesto; set => _idPresupuesto = value; }
    public List<ListadoProductosAltaPresupuestoViewModel> Productos { get => _productos; set => _productos = value; }
    public int IdProductoSeleccionado { get => _idProductoSeleccionado; set => _idProductoSeleccionado = value; }
}
public class ListadoProductosAltaPresupuestoViewModel
{
    private int _idProducto;
    private string _nombreProducto;

    public ListadoProductosAltaPresupuestoViewModel()
    {
    }

    public ListadoProductosAltaPresupuestoViewModel(int id, string nombre)
    {
        _idProducto=id;
        _nombreProducto= nombre;
    }

    public int IdProducto { get => _idProducto; set => _idProducto = value; }
    public string NombreProducto { get => _nombreProducto; set => _nombreProducto = value; }
}