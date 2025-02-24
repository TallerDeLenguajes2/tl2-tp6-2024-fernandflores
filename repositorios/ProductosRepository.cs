using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Data.Sqlite;
using SQLitePCL;
public class ProductoRepository:IProductoRepository
{
    private readonly ILogger _logger;
    private readonly string _connectionString;

    public ProductoRepository(string CadenaDeConexion)
    {
        _connectionString= CadenaDeConexion;
    }

    public ProductoRepository()
    {
    }

    public void CrearProducto (Productos producto)
    {
        try
        {
            string queryString= "INSERT INTO Productos (Descripcion, Precio) VALUES (@descripcion, @precio)"; // consulta sql
            using (SqliteConnection conexion= new SqliteConnection(_connectionString)) // objeto connection sirve para conectar(hacemos la conexion)
            {
                conexion.Open(); //abrimos la conexion gracias al objeto connection
                SqliteCommand comando= new SqliteCommand (queryString, conexion); // clase comando, permite hacer las consultas(recibe como referencia una consulta sql y la conexion)
                comando.Parameters.AddWithValue("@descripcion", producto.Descripcion);
                comando.Parameters.AddWithValue("@precio", producto.Precio); // agregamos los parametros gracias a la clase comand
                comando.ExecuteNonQuery(); // ejectua inset update y delete
                conexion.Close(); //cerramos la conexion con el objeto connection
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw new Exception();
        }
    }
    public void ModificarProducto (int id, Productos producto)
    {
        try
        {
            string queryString= "UPDATE Productos SET Descripcion= @descripcion, Precio= @precio WHERE idProducto=@id";
            using (SqliteConnection connection= new SqliteConnection(_connectionString))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand (queryString, connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@descripcion", producto.Descripcion);
                command.Parameters.AddWithValue("@precio", producto.Precio);
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
    public List<Productos> ListarProdcutos()
    {
        try
        {
            var productos= new List<Productos>();
            string queryString= "SELECT idProducto, Descripcion, Precio FROM Productos";
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand (queryString, connection);
                using (SqliteDataReader reader= command.ExecuteReader()) //uso de la clase DataReader que permite leer datos
                {
                    while (reader.Read()) //recorremos la tabla sql gracias a la clase DataReader
                    {
                        var productoAux= new Productos();
                        productoAux.IdProducto= Convert.ToInt32(reader["idProducto"]); // reader[""]permite obtener la lectura de ese elemento especifico de la tabla
                        productoAux.Precio= Convert.ToInt32(reader["Precio"]);
                    //  productoAux.Descripcion= reader["Descripcion"].ToString(); es lo mismo solo que en caso de ser null devuelve una exepcion
                        productoAux.Descripcion= Convert.ToString(reader["Descripcion"]); // en caso de ser null devuelve vacio " "
                        productos.Add(productoAux);                    
                    }
                }
                connection.Close();
            }
            return productos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw new Exception();
        }
    }
    public Productos ObtenerProductoPorId(int id)
    {
        try
        {
            Productos producto= null;
            string query= "SELECT Precio, Descripcion FROM Productos WHERE idProducto= @id";
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                SqliteCommand command = new SqliteCommand(query, connection);
                connection.Open();
                command.Parameters.AddWithValue("@id", id);
                using (SqliteDataReader reader= command.ExecuteReader())
                {
                if (reader.Read())
                    {
                        producto= new Productos();
                        producto.IdProducto= id;
                        producto.Descripcion= Convert.ToString(reader["Descripcion"]);
                        producto.Precio= Convert.ToInt32(reader["Precio"]);
                    }
                }
                connection.Close();
            }
            return producto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw new Exception();
        }
    }
    public void EliminarPorId (int id)
    {
        try
        {
            string query= "DELETE FROM Productos WHERE idProducto=@id";
            string query2="DELETE FROM PresupuestosDetalle WHERE idProducto=@id";
            using (SqliteConnection connection= new SqliteConnection(_connectionString))
            {
                connection.Open();
                SqliteCommand command= new SqliteCommand(query, connection);
                SqliteCommand command2= new SqliteCommand(query2, connection);
                command2.Parameters.AddWithValue("@id",id);
                command.Parameters.AddWithValue("@id", id);
                command2.ExecuteNonQuery();
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
}
