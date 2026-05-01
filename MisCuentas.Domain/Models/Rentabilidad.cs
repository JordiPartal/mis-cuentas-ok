namespace MisCuentas.Domain.Models;

public class Rentabilidad
{
    public int Ano { get; set; }
    public int Mes { get; set; }
    public string MesTexto { get; set; }
    public decimal Ingresos { get; set; }
    public decimal Gastos { get; set; }  
    public decimal Margen { get; set; }
    public decimal MargenPorcentaje { get; set; }
    public decimal Saldo { get; set; }
    public decimal SuRentabilidad { get; set; }
}