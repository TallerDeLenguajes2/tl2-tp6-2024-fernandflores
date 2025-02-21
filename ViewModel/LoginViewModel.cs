using System.ComponentModel.DataAnnotations;
public class LoginViewModel
{
    private string _userName;
    private string _password;
    private string _errorMessage; // por si se quiere logear y los datos son incorrectos, guardaremos un mesnaje aqui
    private bool _isAuthenticated;

    public LoginViewModel()
     {
    //     _errorMessage= "error 404";
    //     _isAuthenticated= false;    no usaremos esto como en presupuesto, ya que sino muestra el error al inicio de la vista entrando al if porque ya no estaria vacio _errorMessage
    }

    public LoginViewModel(string UserName, string Pass, string ErrorMenssage, bool IsAuthenticated)
    {
        _userName= UserName;
        _password= Pass;
        _errorMessage= ErrorMenssage;
        _isAuthenticated= IsAuthenticated;
    }
    [Required(ErrorMessage ="debe completar este campo")][MaxLength(100, ErrorMessage= "Maximo de caracteres: 100")]
    public string UserName { get => _userName; set => _userName = value; }
    [Required(ErrorMessage ="debe completar este campo")][MaxLength(100, ErrorMessage= "Maximo de caracteres: 100")]
    public string Password { get => _password; set => _password = value; }
    public string ErrorMessage { get => _errorMessage; set => _errorMessage = value; }
    public bool IsAuthenticated { get => _isAuthenticated; set => _isAuthenticated = value; }
}
