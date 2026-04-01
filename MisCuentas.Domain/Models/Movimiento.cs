namespace MisCuentas.Domain.Models;

public class Movimiento
{
    public int id { get; set; }
    public int ano { get; set; }
    public int mes { get; set; }
    public int dia { get; set; }
    public DateTime cargo { get; set; }
    public string tipo { get; set; }
    public string concepto { get; set; }
    public decimal debe { get; set; }
    public decimal haber { get; set; }
    public decimal saldo { get; set; }
}