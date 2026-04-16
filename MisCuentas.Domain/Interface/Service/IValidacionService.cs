namespace MisCuentas.Domain.Interface;

public interface IValidacionService
{
    public int? ValidarNumero(string mensaje);
    public string ValidarTexto(string mensaje);
    public decimal ValidarDecimal(string mensaje);
    public DateTime ValidarFecha(string mensaje);
}