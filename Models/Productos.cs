public class Productos
{
    private int _idProducto;
    private string _descripcion;
    private int _precio;

    public Productos()
    {
    }
    public Productos(int id, string descrip, int precio)
    {
        _idProducto=id;
        _descripcion+=descrip;
        _precio=precio;
    }

    public int IdProducto { get => _idProducto; set => _idProducto = value; }
    public string Descripcion { get => _descripcion; set => _descripcion = value; }
    public int Precio { get => _precio; set => _precio = value; }
    
}