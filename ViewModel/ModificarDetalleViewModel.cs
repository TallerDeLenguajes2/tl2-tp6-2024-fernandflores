using System.ComponentModel.DataAnnotations; 
public class ModificarDetalleViewModel
{
    private List<ProductoViewModel> _listaProductos;
    private int _cantidad;
    private int _idProductoSeleccionado;
    private int _idPresupuesto;
    private int _idProdAnterior;

    public ModificarDetalleViewModel()
    {
        _listaProductos= new List<ProductoViewModel>();
    }

    public ModificarDetalleViewModel(List<ProductoViewModel> listaProductos, int cant, int idProdSeleccionado, int idPres, int idProdAnt)
    {
        _listaProductos= listaProductos;
        _cantidad= cant;
        _idProductoSeleccionado= idProdSeleccionado;
        _idPresupuesto= idPres;
        _idProdAnterior= idProdAnt;
    }   

    public List<ProductoViewModel> ListaProductos { get => _listaProductos; set => _listaProductos = value; }
    [Required][Range(1,50, ErrorMessage ="maximo 50")]
    public int Cantidad { get => _cantidad; set => _cantidad = value; }
    [Required][Range(1,int.MaxValue, ErrorMessage ="Debe ser un numero entero y perteneciente a los id disponibles")]
    public int IdProductoSeleccionado { get => _idProductoSeleccionado; set => _idProductoSeleccionado = value; }
    public int IdPresupuesto { get => _idPresupuesto; set => _idPresupuesto = value; } 
    public int IdProdAnterior { get => _idProdAnterior; set => _idProdAnterior = value; }
}