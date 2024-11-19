using Microsoft.AspNetCore.Mvc;

public class LoginController : Controller
{
    private readonly inMemoryUserRepository _inMemoryUserRepository;
    public LoginController(inMemoryUserRepository inMemoryUserRepository)
    {
        _inMemoryUserRepository = inMemoryUserRepository;
    }
    public IActionResult Index()
    {
        var model = new LoginViewModel
        {
            IsAuthenticated= false
        };
        return View(model);
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