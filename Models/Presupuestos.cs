public class Presupuestos{
    private int _idPresupuesto;
    private Clientes _cliente;
    private List<PresupuestosDetalle> _detalle;
    private DateTime _fechaCreacion;
    public int IdPresupuesto { get => _idPresupuesto; set => _idPresupuesto = value; }
    public List<PresupuestosDetalle> Detalle { get => _detalle; set => _detalle = value; }
    public DateTime FechaCreacion { get => _fechaCreacion; set => _fechaCreacion = value; }
    public Clientes Cliente { get => _cliente; set => _cliente = value; }

    public Presupuestos()
    {
    }
    public Presupuestos(int id, Clientes Clte, List<PresupuestosDetalle> lista, DateTime fecha)
    {
        _idPresupuesto= id;
        _cliente= Clte;
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
