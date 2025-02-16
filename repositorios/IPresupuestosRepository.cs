public interface IPresupuestosRepository
{
    void CrearPresupuesto (Presupuestos presupuesto);
    List<Presupuestos> ListarPresupuestos();
    Presupuestos ObtenerPresupuestoPorId (int id);
    bool AgregarProductoyCantidad(int idPres, int idProd, int cant);
    bool EliminarPresupuesto (int id);
    bool ModificarDetalle(PresupuestosDetalle detalle, int idPres ,int idProdViejo);
    bool ModificarPresupuesto(Presupuestos presupuesto, int id);
}