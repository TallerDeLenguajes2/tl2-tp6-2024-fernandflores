public class PresupuestoAltaViewModel
{
    public DateTime fecha;
    public List<ClienteViewModel> clientes;
    public List<ProductoViewModel> productos;

    public PresupuestoAltaViewModel(List<ClienteViewModel> cltes, List<ProductoViewModel> prodtos)
    {
        this.fecha= (DateTime.Now).Date;
        clientes=cltes;
        productos= prodtos;
    }
}
public class ClienteViewModel
{
    public int clienteId;
    public string nombre;

    public ClienteViewModel()
    {
    }

    public ClienteViewModel(int id, string nombre)
    {
        clienteId=id;
        this.nombre=nombre;
    }
}
public class ProductoViewModel
{
    public int idProd;
    public string descripcion;

    public ProductoViewModel(int id, string descrip)
    {
        idProd= id;
        descripcion= descrip;
    }
}