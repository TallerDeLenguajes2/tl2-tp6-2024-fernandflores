using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

public class User
{
    private int _id;
    private string _userName;
    private string _password;
    private string _nombre;
    private AccesLevel _accesLevel;

    public User()
    {
    }
   
    public User(int Id, string usuario, string contrasena, string nombre, AccesLevel rol)
    {
        _id=Id;
        _userName= usuario;
        _password= contrasena;
        _nombre= nombre;
        _accesLevel= rol;
    }
    public int Id { get => _id; set => _id = value; }
    public string UserName { get => _userName; set => _userName = value; }
    public string Password { get => _password; set => _password = value; }
    public string Nombre { get => _nombre; set => _nombre = value; }
    public AccesLevel AccesLevel { get => _accesLevel; set => _accesLevel = value; }
}
public enum AccesLevel 
{
    [Display(Name = "Administrador")]
    admin,
    [Display(Name = "Cliente")]
    cliente
}