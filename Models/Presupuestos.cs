public class Presupuestos{
    private int _idPresupuesto;
    private string _nombreDestinatario;
    private List<PresupuestosDetalle> _detalle;
    private DateTime _fechaCreacion;
    public int IdPresupuesto { get => _idPresupuesto; set => _idPresupuesto = value; }
    public string NombreDestinatario { get => _nombreDestinatario; set => _nombreDestinatario = value; }
    public List<PresupuestosDetalle> Detalle { get => _detalle; set => _detalle = value; }
    public DateTime FechaCreacion { get => _fechaCreacion; set => _fechaCreacion = value; }

    public Presupuestos()
    {
    }
    public Presupuestos(int id, string nombre, List<PresupuestosDetalle> lista, DateTime fecha)
    {
        _idPresupuesto= id;
        _nombreDestinatario= nombre;
        _detalle= lista;
        _fechaCreacion=fecha;
    }
    public double MontoPresupuesto()
    {
        double monto= _detalle.Sum(d=>d.Cantidad * d.Producto.Precio);
        return monto;
    }
    public double MontoPresupuestoConIva()
    {
        double montoConIva= MontoPresupuesto()*1.21;
        return montoConIva;
    }
    public int CantidadProductos()
    {
        int cant= _detalle.Sum(d=>d.Cantidad);
        return cant;
    }
}
