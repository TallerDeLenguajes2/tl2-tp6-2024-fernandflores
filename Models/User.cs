public class User
{
    public int id{get; set;}
    public string userName {get;set;} = string.Empty;
    public string password {get; set;} = string.Empty;
    public AccesLevel accesLevel {get; set;}

}
public enum AccesLevel 
{
    admin,
    invitado,
    comun
}