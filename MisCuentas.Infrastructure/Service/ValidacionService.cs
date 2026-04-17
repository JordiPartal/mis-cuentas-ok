using System.Globalization;
using MisCuentas.Domain.Enums;
using MisCuentas.Domain.Interface.Service;

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
            
        if (DateTime.TryParseExact(
                entrada,
                "dd-MM-yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime resultado))
        {
            return resultado;
        }
        
        Console.WriteLine("Formato inválido, se usará la fecha actual.");
        return DateTime.Now; 
    }
    
    public int ValidarTipo(string mensaje)
    {
        Console.Write(mensaje);
        string entrada = (Console.ReadLine() ?? string.Empty).Trim().ToUpper();
        
        return entrada switch
        {
            "NOMINA"   => (int)tipoTransaccion.Nomina,
            "RENTING"  => (int)tipoTransaccion.Renting,
            "TRANSPORTE" => (int)tipoTransaccion.Transporte,
            "OCIO"     => (int)tipoTransaccion.Ocio,
            "INGRESO"  => (int)tipoTransaccion.Ingreso,
            _          => (int)tipoTransaccion.Simple
        };
    }

    public int ValidarImpuesto(string mensaje)
    {
        Console.Write(mensaje);
        int entrada = int.Parse(Console.ReadLine() ?? string.Empty);

        return entrada switch
        {
            0 => (int)tipoImpuesto.SIN,
            4 => (int)tipoImpuesto.IVA4,
            10 => (int)tipoImpuesto.IVA10,
            21 => (int)tipoImpuesto.IVA21,
            15 => (int)tipoImpuesto.IRPF15,
            _ => (int)tipoImpuesto.SIN
        };
    }
}