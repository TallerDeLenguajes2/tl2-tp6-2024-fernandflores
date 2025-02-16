public class ProductoViewModel
{
    private int _idProducto;
    private string _descripcion;

    public int IdProducto { get => _idProducto; set => _idProducto = value; }
    public string Descripcion { get => _descripcion; set => _descripcion = value; }

    public ProductoViewModel(int id, string descrip)
    {
        _idProducto= id;
        _descripcion= descrip;
    }
}