namespace MisCuentas.Domain.Models;

public class Transaccion
{ 
    public DateTime fechaCargo { get; set; }
    public string tipo { get; set; }
    public string concepto { get; set; }
    public decimal _base { get; set; }
    public decimal cuota { get; set; }
    public decimal cantidad { get; set; }
    public int idtipo { get; set; }
    public int? idImpuesto { get; set; }
}