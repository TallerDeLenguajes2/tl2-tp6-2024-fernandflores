public class inMemoryUserRepository
{
    private readonly List<User> _users;

    public inMemoryUserRepository()
    {
        _users = new List<User>
        {
            new User{id=1, userName="administrador", password= "contra123", accesLevel= AccesLevel.admin},
            new User{id=2, userName="test", password="1234", accesLevel= AccesLevel.comun}
        };
    }
    public User GetUser (string usuario, string password)
    {
        return _users.Where(u=>u.userName.Equals(usuario, StringComparison.OrdinalIgnoreCase) && u.password.Equals(password, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
    }
}