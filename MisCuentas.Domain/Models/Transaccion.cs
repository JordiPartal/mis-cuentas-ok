namespace MisCuentas.Domain.Models;

public class Transaccion
{ 
    public int? Id { get; set; }
    public DateTime FechaCargo { get; set; }
    public string Tipo { get; set; }
    public string Concepto { get; set; }
    public decimal BaseImponible { get; set; }
    public decimal Cuota { get; set; }
    public decimal Cantidad { get; set; }
    public int IdTipo { get; set; }
    public int? IdImpuesto { get; set; }
}