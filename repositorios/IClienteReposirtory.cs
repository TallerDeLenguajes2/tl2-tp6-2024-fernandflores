public interface IClienteRepository
{
    void CrearCliente (Clientes cliente);
    List<Clientes> ListarClientes();
    Clientes ObtenerClientePorId (int id);
    void ModificarCliente(Clientes cliente);
    void EliminarCliente(int id);
    

}