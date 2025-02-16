using System.ComponentModel.DataAnnotations;

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
    [Required(ErrorMessage ="debe ingresar un nombre")][StringLength(100, ErrorMessage ="maximo 100 caracteres")]
    public string Nombre { get => _nombre; set => _nombre = value; }
    [Required (ErrorMessage ="debe ingresar un mail")][EmailAddress(ErrorMessage ="formato de correo incorrecto")]
    public string Email { get => _email; set => _email = value; }
    [Required (ErrorMessage ="debe ingresar un telefono")][Phone(ErrorMessage ="formato de telefono incorrecto")]
    public string Telefono { get => _telefono; set => _telefono = value; }
}