using MisCuentas.Domain.Interface;

namespace MisCuentas.Infrastructure.Service;

public class ValidacionService : IValidacionService
{
    public int? ValidarNumero(string mensaje)
    {
        Console.Write(mensaje);
        string? entrada = Console.ReadLine();
        return int.TryParse(entrada, out int resultado) ? resultado : null;
    }

    public string ValidarTexto(string mensaje)
    {
        Console.Write(mensaje);
        string? cadena = Console.ReadLine();
        if (cadena.Length > 0) return cadena;
        return string.Empty;
    }

    public decimal ValidarDecimal(string mensaje)
    {
        Console.Write(mensaje);
        string? entrada = Console.ReadLine();
        return decimal.TryParse(entrada, out decimal resultado) ? resultado : 0;
    }
    
    public DateTime ValidarFecha(string mensaje)
    {
        Console.Write(mensaje);
        string? entrada = Console.ReadLine();
        return DateTime.TryParse(entrada, out DateTime resultado) ? resultado : DateTime.Now;
    }   
}