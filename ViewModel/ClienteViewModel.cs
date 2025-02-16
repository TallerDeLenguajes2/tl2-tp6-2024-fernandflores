public class ClienteViewModel
{
    private int _clienteId;
    private string _nombre;

    public int ClienteId { get => _clienteId; set => _clienteId = value; }
    public string Nombre { get => _nombre; set => _nombre = value; }

    public ClienteViewModel()
    {
    }

    public ClienteViewModel(int id, string nombre)
    {
        ClienteId=id;
        this.Nombre=nombre;
    }
}
