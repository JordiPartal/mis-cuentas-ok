namespace MisCuentas.Domain.Models;

public class EstadoFecha
{
    public int Mes { get; set; } = DateTime.Now.Month;
    public int Anio { get; set; } = DateTime.Now.Year;
    public event Action AlCambiarEstado;
    public void CambiarEstado(int mes, int anio)
    {
        Mes = mes;
        Anio = anio;
        NotificarCambio();
    }
    private void NotificarCambio() => AlCambiarEstado?.Invoke();
}