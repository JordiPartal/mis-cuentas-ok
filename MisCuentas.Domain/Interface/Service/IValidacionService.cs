namespace MisCuentas.Domain.Interface.Service;

public interface IValidacionService
{
    public int? ValidarNumero(string mensaje);
    public string ValidarTexto(string mensaje);
    public decimal ValidarDecimal(string mensaje);
    public DateTime ValidarFecha(string mensaje);
    public int ValidarTipo(string mensaje);
    public int ValidarImpuesto(string mensaje);
}