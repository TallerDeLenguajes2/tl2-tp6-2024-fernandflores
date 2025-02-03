using Microsoft.Data.Sqlite;

public class ClientesRepository
{
    public void CrearCliente(Clientes cliente)
    {
        string connectionString= "Data Source= Tienda.db; Cache= Shared";
        string query="INSERT INTO Clientes (Nombre, Email, Telefono) VALUES (@nombre, @email, @tel)";
        using (var connection= new SqliteConnection(connectionString))
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
    public List<Clientes> ListarClientes()
    {
        var lista= new List<Clientes>();
        string connectionString="Data Source= Tienda.db; Cache= Shared";
        string query = "SELECT ClienteId, Nombre, Email, Telefono FROM Clientes";
        using (var connection= new SqliteConnection(connectionString))
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
    public Clientes ObtenerClientePorId (int id)
    {
        Clientes cliente= null;
        string connectionString="Data Source= Tienda.db; Cache= Shared";
        string query = "SELECT ClienteId, Nombre, Email, Telefono FROM Clientes WHERE ClienteId=@id";
        using (var connection= new SqliteConnection(connectionString))
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
    public void ModificarCliente(Clientes cliente)
    {
        string connectionString="Data Source= Tienda.db; Cache= Shared";
        string query= "UPDATE Clientes SET Nombre=@nombre, Email= @email, Telefono=@tel WHERE ClienteId=@id";
        using(var connection= new SqliteConnection(connectionString))
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
    public void EliminarCliente(int id)
    {
        var repoPresupuesto= new PresupuestosRepository();
        string connectionString = "Data Source= Tienda.db; Cache= Shared";
        string query= "DELETE FROM Clientes WHERE ClienteId = @id";
        string query2= "SELECT idPresupuesto FROM Presupuestos WHERE ClienteId= @id";
        using (var connection= new SqliteConnection(connectionString))
        {
            connection.Open();
            var command= new SqliteCommand(query, connection);
            var command2= new SqliteCommand(query2, connection);
            command2.Parameters.AddWithValue("@id", id); // asocio el id al query2
            int idPres= Convert.ToInt32(command2.ExecuteScalar()); // obtengo el idpresupuesto buscado en el query2 (execute escalar nos permite ejecutar una consulta que devuelve un solo valor)
            repoPresupuesto.EliminarPresupuesto(idPres); // elimino el presupuesto del cliente
            command.Parameters.AddWithValue("@id", id); // asocio el id al query
            command.ExecuteNonQuery(); // ejecuto la consulta query
            connection.Close();
        }
    }
}