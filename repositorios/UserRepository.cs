using Microsoft.Data.Sqlite;
using SQLitePCL;
public class UserRepository:IUserRepository
{
    private readonly ILogger _logger;
    private readonly string _connectionString;
    private Seguridad _security;
    public UserRepository(string CadenaDeConexion)
    {
        _connectionString= CadenaDeConexion;
        _security= new Seguridad();
    }
    public bool ValidarNombreUsuario(string nombreUsuario)
    {
        try
        {
            var ListaDeNombresUsuario= new List<string>();
            string query= "SELECT Usuario FROM Usuarios";
            using (var connection= new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command= new SqliteCommand(query, connection);
                using (SqliteDataReader reader= command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        ListaDeNombresUsuario.Add(Convert.ToString(reader["Usuario"]));
                    }
                    connection.Close();
                }
            }
            return !ListaDeNombresUsuario.Contains(nombreUsuario);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw new Exception();
        }
    }
    public void CrearUsuario(User usuario)
    {
        try
        {
            string PassHasheada= _security.HashPassword(usuario.Password);
            string query= "INSERT INTO Usuarios (Nombre, Usuario, Contrasena, IdRol) VALUES (@nombre, @usuario, @contrasena, @rol)";
            using (SqliteConnection connection= new SqliteConnection (_connectionString))
            {
                connection.Open();
                var command= new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@nombre", usuario.Nombre);
                command.Parameters.AddWithValue("@usuario", usuario.UserName);
                command.Parameters.AddWithValue("@contrasena", PassHasheada);
                command.Parameters.AddWithValue("@rol", (int)usuario.AccesLevel);
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
    public User ValidarLogeo (string usuario, string passIngresada)
    {
        try
        {
            var query= "SELECT Contrasena FROM Usuarios WHERE Usuario=@usu";
            var query2= "SELECT * FROM Usuarios WHERE Usuario=@usuario";
            using (SqliteConnection connection= new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command= new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@usu", usuario);
                string ContrasenaGuardada= Convert.ToString(command.ExecuteScalar());
                if (_security.ValidarContrasena(ContrasenaGuardada, passIngresada)) //si las contraseñas coinciden obtengo todos los datos
                {
                    var command2= new SqliteCommand(query2, connection);
                    command2.Parameters.AddWithValue("@usuario", usuario);
                    using (SqliteDataReader reader = command2.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            var rol= (AccesLevel)Convert.ToInt32(reader["IdRol"]);
                            var user= new User(Convert.ToInt32(reader["Id"]), Convert.ToString(reader["Usuario"]), Convert.ToString(reader["Contrasena"]), Convert.ToString(reader["Nombre"]), rol);
                            connection.Close();
                            return user;
                        }
                    }
                }
            }
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw new Exception();
        }
    }
}