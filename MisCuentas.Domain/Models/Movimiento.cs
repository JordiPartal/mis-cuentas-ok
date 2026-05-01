namespace MisCuentas.Domain.Models;

public class Movimiento
{
    public int Id { get; set; }
    public int Ano { get; set; }
    public int Mes { get; set; }
    public int Dia { get; set; }
    public DateTime Cargo { get; set; }
    public string Tipo { get; set; }
    public string Concepto { get; set; }
    public decimal Debe { get; set; }
    public decimal Haber { get; set; }
    public decimal Saldo { get; set; }
}