public class Clientes
{
    private int _clienteId;
    private string _nombre;
    private string _email;
    private string _telefono;

    public Clientes()
    {
    }

    public Clientes(int id, string nombre, string email, string tel)
    {
        _clienteId=id;
        _nombre= nombre;
        _email= email;
        _telefono= tel;
    }

    public int ClienteId { get => _clienteId; set => _clienteId = value; }
    public string Nombre { get => _nombre; set => _nombre = value; }
    public string Email { get => _email; set => _email = value; }
    public string Telefono { get => _telefono; set => _telefono = value; }
}