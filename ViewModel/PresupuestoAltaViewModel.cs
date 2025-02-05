public class PresupuestoAltaViewModel
{

    private List<ClienteViewModel> _clientes;
    private List<ProductoViewModel> _productos;
    private int _idClienteSeleccionado;
    private int _idProductoSeleccionado;

    public PresupuestoAltaViewModel()
    {
    }

    public PresupuestoAltaViewModel(List<ClienteViewModel> cltes, List<ProductoViewModel> prodtos)
    {
        
        _clientes=cltes;
        _productos= prodtos;
    }

    public List<ProductoViewModel> Productos { get => _productos; set => _productos = value; }
    public List<ClienteViewModel> Clientes { get => _clientes; set => _clientes = value; }
    public int IdClienteSeleccionado { get => _idClienteSeleccionado; set => _idClienteSeleccionado = value; }
    public int IdProductoSeleccionado { get => _idProductoSeleccionado; set => _idProductoSeleccionado = value; }
}
public class ClienteViewModel
{
    private int _clienteId;
    private string _nombre;

    public int ClienteId { get => _clienteId; set => _clienteId = value; }
    public string Nombre { get => _nombre; set => _nombre = value; }

    public ClienteViewModel()
    {
    }

    public ClienteViewModel(int id, string nombre)
    {
        ClienteId=id;
        this.Nombre=nombre;
    }
}
public class ProductoViewModel
{
    private int _idProd;
    private string _descripcion;

    public int IdProd { get => _idProd; set => _idProd = value; }
    public string Descripcion { get => _descripcion; set => _descripcion = value; }

    public ProductoViewModel(int id, string descrip)
    {
        IdProd= id;
        Descripcion= descrip;
    }
}