using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;

public class LoginController : Controller
{
    private readonly IUserRepository _repoUser;
    private readonly ILogger _logger;

    public LoginController(ILogger<LoginController> logger, IUserRepository UserRepository)
    {
        _logger= logger;
        _repoUser= UserRepository;
    }
    [HttpGet]
    public IActionResult Index() // es el formulario de logeo
    {
        var viewmodel = new LoginViewModel //creo un objeto viewmodel
        { 
            IsAuthenticated= HttpContext.Session.GetString("isAuthenticated")=="true" //si hay alguien logeado, le asigno ese valor "true o false" al IsAtuthenticated
        }; // se crea a la vez el objeto viewmodel ya que sino seria null y razor no lo permite
        if (viewmodel.IsAuthenticated)
        {
            viewmodel.UserName= HttpContext.Session.GetString("usuario");
        } 
        ViewBag.Rol=HttpContext.Session.GetString("rol");
        return View(viewmodel);
    }
    [HttpPost]
    public IActionResult Login(LoginViewModel viewmodel)
    {
        ModelState.Remove("ErrorMessage");
        ModelState.Remove("IsAuthenticated"); // como solo el formulario envia pass y usuario a esto lo podemos ignorar del modestate y evitar el error de razon por enviarse nulos estos valores desde el formulario 
        if(!ModelState.IsValid) return RedirectToAction("Index");
        var usuario= _repoUser.ValidarLogeo(viewmodel.UserName,viewmodel.Password);
        if (usuario!=null)
        {
            HttpContext.Session.SetString("isAuthenticated", "true"); //"isAuthenticated" este pertenece a las variables de sesion, mientas que los del modelstate pertenecen al viewmodel (por eso los puse distintos)
            HttpContext.Session.SetString("usuario", usuario.UserName);   //se crea una variable session "usuario"
            HttpContext.Session.SetString("nombre", usuario.Nombre);   // se crea una variable session "nombre"
            HttpContext.Session.SetString("rol",usuario.AccesLevel.ToString());
            _logger.LogInformation("el usuario: "+usuario.UserName + "se ha logeado");
            return RedirectToAction("Index", "Presupuestos");
        }
        _logger.LogWarning("Intento fallido de ingreso a la session, usuario: "+ viewmodel.UserName +" contraseña utilizada: "+viewmodel.Password);
        viewmodel.ErrorMessage="datos incorrectos";
        viewmodel.IsAuthenticated= false;
        return View("Index", viewmodel); 
    }
   
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult CrearUsuario()
    {
        var viewmodel= new CrearUsuarioViewModel();
        return View(viewmodel);
    }
    [HttpPost]
    public IActionResult CrearUsuario(CrearUsuarioViewModel viewmodel)
    {
        if (!ModelState.IsValid) return RedirectToAction("CrearUsuario");
        var validarUsuario= _repoUser.ValidarNombreUsuario(viewmodel.Usuario);
        if(validarUsuario)
        {
            var usuario= new User(0, viewmodel.Usuario, viewmodel.Pass, viewmodel.Nombre, viewmodel.Rol);
            _repoUser.CrearUsuario(usuario);
            return RedirectToAction("Index"); 
        }
        ModelState.AddModelError("Usuario", "Este usuario ya existe");
        return View("CrearUsuario", viewmodel); // envio de nuevo a la vista pero con el viewmodel cosa de no perder los datos
    }
//     [HttpPost]
//     public IActionResult Login(LoginViewModel model)
//     {
        
//         if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
//         {
//             model.ErrorMessage = "Por favor ingrese su nombre de usuario y contraseña.";
//             return View("Index", model);
//         }
        
//     }
 }