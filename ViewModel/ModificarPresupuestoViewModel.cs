using System.ComponentModel.DataAnnotations; 
public class ModificarPresupuestoViewModel
{
    private List <ClienteViewModel> _clientes;
    private int _idClienteSeleccionado;
    private DateTime _fecha;
private int _idPresupuesto;

    public ModificarPresupuestoViewModel()
    {
        _clientes= new List<ClienteViewModel>();
    }

    public ModificarPresupuestoViewModel(int idpres, List<ClienteViewModel> Clientes, DateTime fecha, int idSeleccionado)
    {
        _idPresupuesto= idpres;
        _clientes= Clientes;
        _fecha= fecha; 
        _idClienteSeleccionado= idSeleccionado;
    }

    public List<ClienteViewModel> Clientes { get => _clientes; set => _clientes = value; }
    [Required(ErrorMessage ="fecha es un campo obligarorio")]
    public DateTime Fecha { get => _fecha; set => _fecha = value; }
    [Required][Range(1, int.MaxValue, ErrorMessage ="debe ser un id perteneciente a la BD")]
    public int IdClienteSeleccionado { get => _idClienteSeleccionado; set => _idClienteSeleccionado = value; }
    public int IdPresupuesto { get => _idPresupuesto; set => _idPresupuesto = value; }
}
