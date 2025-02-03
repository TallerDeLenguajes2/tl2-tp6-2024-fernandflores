using Microsoft.Data.Sqlite;
using SQLitePCL;
public class PresupuestosRepository
{
    public void CrearPresupuesto (Presupuestos presupuesto)
    {
        string connectionString= "Data Source= Tienda.db; Cache= Shared";
        string query= "INSERT INTO Presupuestos (ClienteId, FechaCreacion) VALUES (@ClteId, @fecha)";
        string query2= "INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPres, @idProd, @cant)";
        string query3= "SELECT MAX (idPresupuesto) FROM Presupuestos";
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            SqliteCommand command2= new SqliteCommand(query2, connection);
            SqliteCommand command3= new SqliteCommand(query3, connection);
            command.Parameters.AddWithValue("@ClteId", presupuesto.Cliente.ClienteId);
            command.Parameters.AddWithValue("@fecha", presupuesto.FechaCreacion);
            command.ExecuteNonQuery(); // guardamos el presupuesto, ahora debemos obtener el id generado por la base de datos para poder guardarlo en el detalle
            int idGuardado= Convert.ToInt32(command3.ExecuteScalar()); // funcion que nos permite ejecutar una consulta sql que devuelve un solo resultado o valor (aca por ejemplo solo nos devuelve un numero: idPresupuesto)
            foreach (var item in presupuesto.Detalle)
            {
                command2.Parameters.AddWithValue("@idPres", idGuardado); // idPresupuesto al ser clave foranea debe ser un idPresupuesto que exista en la tabla Presupuesto. como estamos guardando el presupuestoDetalle, el id con el que trabajamos es con el del presupuesto que recien se guardo
                command2.Parameters.AddWithValue("@idProd", item.Producto.IdProducto);
                command2.Parameters.AddWithValue("@cant", item.Cantidad);
                command2.ExecuteNonQuery();
            }
            connection.Close();
        }
    }
    public List<Presupuestos> ListarPresupuestos()
    {
        var listaPresupuestos= new List<Presupuestos>();
        string connectionString="Data Source= Tienda.db; Cache= Shared";
        string query= @"SELECT 
                            Pres.idPresupuesto, 
                            Pres.ClienteId, 
                            Pres.FechaCreacion, 
                            Prod.idProducto, 
                            Prod.Descripcion, 
                            Prod.Precio, 
                            PresDet.Cantidad,  
                            Clte.Nombre,
                            Clte.Email,
                            Clte.Telefono

                        FROM 
                            Presupuestos Pres 
                        JOIN 
                            PresupuestosDetalle PresDet ON PresDet.idPresupuesto = Pres.idPresupuesto 
                        JOIN 
                            Productos Prod ON Prod.idProducto = PresDet.idProducto
                        JOIN 
                            Clientes Clte ON Clte.ClienteId = Pres.ClienteId
                        ORDER BY
                            Pres.idPresupuesto";
        // FROM Presupuestos Pres --> tabla principal, se le da el alias Pres para referirnos a ella
        // JOIN PresupuestoDetalle PresDet --> PresDet sera el alias de la tabla PresupuestoDetalle
        // ON PresDet.IdPresupuesto = Pres.idPresupuesto --> se unen las fias que cumplan esa condicion
        // resumen --> join dice: dame las filas donde el idpresupuesto sea igual en la tabla presupuesto y presupuestodetalle
        // JOIN producto prod on prod.idproducto = presdet.idproducto --> como el idproducto es lo que une a esas tablas uso esto para hacer el join
        // ORDER BY pres.idpresupuesto --> ordena lo obtenido en el select por el idpresupuesto, ej de salida id: 1 2 3  

        // el join hace uuna combinacion de todas las filas por lo que vamos a tener idpresupuesto mas de una vez en algunos casos por ejemplo si idpresupuesto "4" tiene 2 detalles vamos a tener dos veces idpresupuesto "4" por la combinacion de las filas (basicamente el resultado del select es una supertabla con todos los valores):
        // idpresupuesto | nombredestinatario | fechacreacion | idproducto descripcion | precio | cantidad
        //    1                 test                7-11-24         mayonesa              400       2
        //    4                 fer                 12-11-24        minipimer             57000      1
        //    4                 fer                 12-11-24        licuadora             40000      1
        // idpresupuesto "1" tiene un solo detalle entonces aparece una vez pero idpresupuesto "4" tiene dos detalles entonces al hacer la combinacion de filas el select nos devuelve 2 veces el id (por eso usamos el if mas adelante)
        // si bien en la base de datos los datos de presupuesto solo son idpres, fehca, clienteid debemos crear los objetos compleots por eso es que se hace una consulta completa
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand comand = new SqliteCommand(query, connection);
            using ( SqliteDataReader reader = comand.ExecuteReader())
            {
                int idPresupuestoAux= -1; // para que cree una sola vez un presupuesto 
                Presupuestos presupuesto = null;
                while(reader.Read())
                
                { //si un presupuesto tiene 2 detalles no queremos que se cree 2 objetos presupuestos por eso el siguiente if
                    if(idPresupuestoAux != Convert.ToInt32(reader["idPresupuesto"]))// aseguramos que cree una sola vez el presupuesto por id
                    {
                        presupuesto= new Presupuestos(Convert.ToInt32(reader["idPresupuesto"]),new Clientes(Convert.ToInt32(reader["ClienteId"]),Convert.ToString(reader["Nombre"]), Convert.ToString(reader["Email"]), Convert.ToString(reader["Telefono"])), new List<PresupuestosDetalle>(), Convert.ToDateTime(reader["FechaCreacion"]));
                        listaPresupuestos.Add(presupuesto);
                    }
                    

                    var producto= new Productos(Convert.ToInt32(reader["idProducto"]),Convert.ToString(reader["Descripcion"]),Convert.ToInt32(reader["Precio"])); //creamos el producto

                    var detalle= new PresupuestosDetalle(producto,Convert.ToInt32(reader["Cantidad"])); //creamos el detallepresupuesto (aca usamos el producto creado)

                    presupuesto.Detalle.Add(detalle); //agregamos el detalle en la lista de detalles del presupuesto
        
                    idPresupuestoAux= Convert.ToInt32(reader["idPresupuesto"]); // actualizamos el idaux para que si existieran otros detalles del presupuesto no entre al primer if y cree otro objeto presupuesto, esto lo uqe hace es que vaya agregando a la lista del objeto presupuesto ya creado

                }
            }
            connection.Close();
        }
        return listaPresupuestos;
    }
    public Presupuestos ObtenerPresupuestoPorId (int id)
    {
        var connectionString ="Data Source= Tienda.db; Cache=Shared";
        Presupuestos presupuesto= null;
        var query=@"SELECT 
                        p.idPresupuesto,
                        p.ClienteId,
                        p.FechaCreacion,
                        prod.idProducto,
                        prod.Descripcion,
                        prod.Precio,
                        d.cantidad,
                        Clte.Nombre,
                        Clte.Email,
                        Clte.Telefono
                    FROM
                        Presupuestos p
                    JOIN
                        PresupuestosDetalle d ON d.idPresupuesto = p.idPresupuesto 
                    JOIN
                        Productos prod ON prod.IdProducto = d.idProducto
                    JOIN 
                        Clientes Clte ON Clte.ClienteId = p.ClienteId
                    WHERE
                        p.idPresupuesto=@id";
        int idAux= -1;
        using (var connection= new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command= new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    if(idAux != Convert.ToInt32(reader["idPresupuesto"])) //creo solo un objeto presupuesto(si tiene varios detalles la consulta devuelve varios idpresupuestos (iguales) como demostre en el ejemplo de supertabla)
                    {
                        presupuesto= new Presupuestos(Convert.ToInt32(reader["idPresupuesto"]),new Clientes(Convert.ToInt32(reader["ClienteId"]),Convert.ToString(reader["Nombre"]), Convert.ToString(reader["Email"]), Convert.ToString(reader["Telefono"])), new List<PresupuestosDetalle>(), Convert.ToDateTime(reader["FechaCreacion"]));
                        idAux= Convert.ToInt32(reader["idPresupuesto"]);                    
                    }
                    var producto= new Productos(Convert.ToInt32(reader["idProducto"]), Convert.ToString(reader["Descripcion"]),Convert.ToInt32(reader["Precio"]));
                    var detalle= new PresupuestosDetalle(producto, Convert.ToInt32(reader["Cantidad"]));
                    presupuesto.Detalle.Add(detalle);
                }
            }
            connection.Close();
        }
        return presupuesto;
    }

    public bool AgregarProductoyCantidad(int idPres, int idProd, int cant)
    {  
        ProductoRepository repoProd= new ProductoRepository();
        if(ObtenerPresupuestoPorId(idPres)==null || repoProd.ObtenerProductoPorId(idProd)==null) return false;
        string connectionString= "Data Source= Tienda.db; Cache= Shared";
        string query= "INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPres, @idProd, @cant)";
        using (SqliteConnection connection= new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@idPres", idPres);
            command.Parameters.AddWithValue("@idProd", idProd);
            command.Parameters.AddWithValue("@cant", cant);
            command.ExecuteNonQuery();
            connection.Close();
        }
        return true;
    }
    public bool EliminarPresupuesto (int id)
    {
        if (ObtenerPresupuestoPorId(id)==null)return false;
        string connectionString= "Data Source= Tienda.db; Cache= Shared";
        string query="DELETE FROM Presupuestos WHERE idPresupuesto=@id";
        string query2= "DELETE FROM PresupuestosDetalle WHERE idPresupuesto=@id";
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command= new SqliteCommand(query, connection);
            var command2= new SqliteCommand(query2, connection);
            command2.Parameters.AddWithValue("@id", id);
            command2.ExecuteNonQuery();
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
            connection.Close();

            return true;
        }
    }
    public bool ModificarDetalle(PresupuestosDetalle detalle, int idPres ,int idProdViejo)
    {
        string query="UPDATE PresupuestosDetalle SET idProducto=@idProdNuevo, Cantidad=@cant WHERE idPresupuesto=@idPres AND idProducto=@idProd";
        string connectionString="Data Source= Tienda.db; Cache= Shared";
        using (var connection= new SqliteConnection(connectionString))
        {
            connection.Open();
            var command= new SqliteCommand(query,connection);
            command.Parameters.AddWithValue("@idPres", idPres);
            command.Parameters.AddWithValue("@idProd", idProdViejo);
            command.Parameters.AddWithValue("@idProdNuevo", detalle.Producto.IdProducto);
            command.Parameters.AddWithValue("@cant", detalle.Cantidad);
            int filas=command.ExecuteNonQuery(); // el executenonquer ejectuta el comando devuelve la cantidad de filas afectadas
            connection.Close();
            return filas>0; //devuelve true si las fila es mayor a 0 (hubo modificacion)
        }
    }
    public bool ModificarPresupuesto(Presupuestos presupuesto, int id)
    {
        string query= "UPDATE Presupuestos SET ClienteId= @ClteId, FechaCreacion=@fecha WHERE idPresupuesto=@idPres";
        string connectionString="Data Source= Tienda.db; Cache= Shared";
        using (var connection= new SqliteConnection(connectionString))
        {
            connection.Open();
            var command= new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@idPres", id);
            command.Parameters.AddWithValue("@ClteId", presupuesto.Cliente.ClienteId);
            command.Parameters.AddWithValue("@fecha", presupuesto.FechaCreacion);
            int filas= command.ExecuteNonQuery();
            connection.Close();
            return filas>0;
        }
    }
}
