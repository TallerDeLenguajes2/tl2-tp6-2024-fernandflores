public interface IUserRepository
{
    void CrearUsuario(User usuario);
    User ValidarLogeo (string usuario, string passIngresada);
    bool ValidarNombreUsuario(string nombreUsuario);

}