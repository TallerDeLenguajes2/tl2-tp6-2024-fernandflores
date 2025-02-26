using Microsoft.Data.Sqlite;

public class ClientesRepository:IClienteRepository
{
    private readonly ILogger<ClientesRepository> _logger;
    private readonly string _connectionString;

    public ClientesRepository(string CadenaDeConexion)
    {
        _connectionString= CadenaDeConexion;
    }

    public void CrearCliente(Clientes cliente) 
    {
        try
        {
            string query="INSERT INTO Clientes (Nombre, Email, Telefono) VALUES (@nombre, @email, @tel)";
            using (SqliteConnection connection= new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command= new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@nombre", cliente.Nombre);
                command.Parameters.AddWithValue("@email", cliente.Email);
                command.Parameters.AddWithValue("@tel", cliente.Telefono);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw new Exception();
        }
    }
    public List<Clientes> ListarClientes()
    {
        try
        {
            var lista= new List<Clientes>(); // en los listar no inicializamos en null ya que si es null en la vista daria error (en el controlador a la vista se le envia la lista que sale de aca )
            string query = "SELECT ClienteId, Nombre, Email, Telefono FROM Clientes";
            using (var connection= new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command= new SqliteCommand(query, connection);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    
                    while (reader.Read())
                    {
                        lista.Add(new Clientes(Convert.ToInt32(reader["ClienteId"]), Convert.ToString(reader["Nombre"]), Convert.ToString(reader["Email"]),Convert.ToString(reader["Telefono"])));
                    }
                }
                connection.Close();
            }
            return lista;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw new Exception("error al listar clientes");
        }
    }
    public Clientes ObtenerClientePorId (int id)
    {
        try
        {
            Clientes cliente= null;
            string query = "SELECT ClienteId, Nombre, Email, Telefono FROM Clientes WHERE ClienteId=@id";
            using (var connection= new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command= new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    
                    if (reader.Read())
                    {
                        cliente=new Clientes(Convert.ToInt32(reader["ClienteId"]), Convert.ToString(reader["Nombre"]), Convert.ToString(reader["Email"]),Convert.ToString(reader["Telefono"]));
                    }
                }
                connection.Close();
            }
            return cliente;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw new Exception();
        }
    }
    public void ModificarCliente(Clientes cliente)
    {
        try
        {
            string query= "UPDATE Clientes SET Nombre=@nombre, Email= @email, Telefono=@tel WHERE ClienteId=@id";
            using(var connection= new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command= new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@id", cliente.ClienteId);
                command.Parameters.AddWithValue("@nombre", cliente.Nombre);
                command.Parameters.AddWithValue("@email", cliente.Email);
                command.Parameters.AddWithValue("@tel", cliente.Telefono);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw new Exception();
        }
    }
    public void EliminarCliente(int id)
    {
        try
        {    
            var repoPresupuesto= new PresupuestosRepository(_connectionString);
            string query= "DELETE FROM Clientes WHERE ClienteId = @id";
            string query2= "SELECT idPresupuesto FROM Presupuestos WHERE ClienteId= @id";
            using (var connection= new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command= new SqliteCommand(query, connection);
                var command2= new SqliteCommand(query2, connection);
                command2.Parameters.AddWithValue("@id", id); // asocio el id al query2
             //   int idPres= Convert.ToInt32(command2.ExecuteScalar()); // obtengo el idpresupuesto buscado en el query2 (execute escalar nos permite ejecutar una consulta que devuelve un solo valor)
                repoPresupuesto.EliminarPresupuestoPorCliente(id); // elimino el presupuesto del cliente
                command.Parameters.AddWithValue("@id", id); // asocio el id al query
                command.ExecuteNonQuery(); // ejecuto la consulta query
                connection.Close();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw new Exception();
        }
    }
}