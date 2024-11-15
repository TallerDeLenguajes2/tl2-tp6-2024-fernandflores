public class PresupuestosDetalle
{
    private Productos _producto;
    private int _cantidad;
    public Productos Producto { get => _producto; set => _producto = value; }
    public int Cantidad { get => _cantidad; set => _cantidad = value; }
    public PresupuestosDetalle()
    {
    }
    public PresupuestosDetalle(Productos prod, int cant)
    {
        _producto= prod;
        _cantidad= cant;

    }
}
