using Microsoft.AspNetCore.Identity;
public class Seguridad
{
    private PasswordHasher<object> hasher = new PasswordHasher<object>();
    public string  HashPassword (string Password)
    {
        return hasher.HashPassword(null, Password);
    }
    public bool ValidarContrasena(string contrasenaGuardada, string contrsenaIngresada)
    {
        var resultado= hasher.VerifyHashedPassword(null, contrasenaGuardada, contrsenaIngresada);
        return resultado==PasswordVerificationResult.Success; //retorna true si las contraseñas coinciden
    }
}