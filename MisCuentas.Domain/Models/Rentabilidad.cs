namespace MisCuentas.Domain.Models;

public class Rentabilidad
{
    public int ano { get; set; }
    public int mes { get; set; }
    public string mesTexto { get; set; }
    public decimal ingresos { get; set; }
    public decimal gastos { get; set; }  
    public decimal margen { get; set; }
    public decimal margenPorcentaje { get; set; }
    public decimal saldo { get; set; }
    public decimal rentabilidad { get; set; }
}