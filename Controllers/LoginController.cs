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
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            TempData["MensajeCatch"]= "no se pudo cargar la pagina de logeo";
            return RedirectToAction ("Index");
        }
    }
    [HttpPost]
    public IActionResult Login(LoginViewModel viewmodel)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            TempData["MensajeCatch"]= "no se pudo autenticar el usuario";
            return RedirectToAction ("Index");
        }
    }
   
    public IActionResult Logout()
    {
        try
        { 
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            TempData["MensajeCatch"]= "no se pudo salir de la sesion";
            return RedirectToAction ("Index");
        }
    }
    [HttpGet]
    public IActionResult CrearUsuario()
    {
        try
        {
            var viewmodel= new CrearUsuarioViewModel();
            return View(viewmodel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            TempData["MensajeCatch"]= "no se pudo cargar el formulario de crear usuario";
            return RedirectToAction ("Index");
        }
    }
    [HttpPost]
    public IActionResult CrearUsuario(CrearUsuarioViewModel viewmodel)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            TempData["MensajeCatch"]= "no se pudo crear el usuario";
            return RedirectToAction ("Index");
        }
    }
 }