using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
public class CrearUsuarioViewModel
{
    private string _nombre;
    private string _usuario;
    private string _pass;
    private AccesLevel _rol;

    public CrearUsuarioViewModel()
    {
    }

    public CrearUsuarioViewModel(string nombre, string usuario, string pass, AccesLevel rol)
    {
        _nombre= nombre;
        _usuario= usuario;
        _pass= pass;
        _rol= rol;
    }
    [Required(ErrorMessage ="debe completar este campo")][MaxLength(100, ErrorMessage= "Maximo de caracteres: 100")]
    public string Nombre { get => _nombre; set => _nombre = value; }
    [Required(ErrorMessage ="debe completar este campo")][MaxLength(100, ErrorMessage= "Maximo de caracteres: 100")]
    public string Usuario { get => _usuario; set => _usuario = value; }
    [Required(ErrorMessage ="debe completar este campo")][MaxLength(100, ErrorMessage= "Maximo de caracteres: 100")]
    public string Pass { get => _pass; set => _pass = value; }
    [Required(ErrorMessage ="debe seleccionar una opcion")]
    public AccesLevel Rol { get => _rol; set => _rol = value; }
    public List<SelectListItem>RolesDisponibles{get; set;}= Enum.GetValues(typeof(AccesLevel)).Cast<AccesLevel>().Select(s=> new SelectListItem{ Value = s.ToString(), Text= s.ToString()}).ToList(); //creamos una selectlist (enum no lo es, una lista si) para que podamos usar asp-item y hacer el select de manera mas prolija
}