using System.Globalization;
using MisCuentas.Domain.Enums;

namespace MisCuentas.Infrastructure.Tmp.Utils;

public class Validacion
{
    /// <summary>
    /// Valida que el dato sea un entero o un valor nulo
    /// </summary>
    /// <param name="mensaje"></param>
    /// <returns></returns>
    public static int? LeerEnteroOpcional(string mensaje)
    {
        Console.Write(mensaje);
        string? entrada = Console.ReadLine();
        return int.TryParse(entrada, out int resultado) ? resultado : null;
    }
    
    /// <summary>
    /// Valida que la confirmación sea si o no
    /// </summary>
    /// <param name="mensaje"></param>
    /// <returns></returns>
    public static int? LeerInput(string mensaje)
    {
        Console.Write(mensaje);
        string? opcion = Console.ReadLine();
        if (opcion == "si".ToLower()) return 0;
        return 1;
    }
    
    /// <summary>
    /// Valida que el dato sea un string válida
    /// </summary>
    /// <param name="dato"></param>
    /// <returns></returns>
    public static string ValidarString(string dato)
    {
        Console.Write(dato);
        var cadena = Console.ReadLine();
        
        if (cadena.Length > 0) return cadena;
        return string.Empty;
    }
    
    /// <summary>
    /// Valida que el dato sea una fecha valida en formato dd-MM-yyyy
    /// </summary>
    /// <returns></returns>
    public static DateTime ValidarFecha()
    {
        Console.Write("Fecha: ");
        var input = Console.ReadLine();
            
        return DateTime.TryParseExact(
            input,
            "dd-MM-yyyy",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out DateTime validado
        ) ? validado : DateTime.Today;
    }
    
    /// <summary>
    /// Valida que el dato sea un decimal válido
    /// </summary>
    /// <returns></returns>
    public static decimal ValidarNumero()
    {
        try
        {
            Console.Write("Cantidad: ");
            var input = Console.ReadLine();
            decimal.TryParse(input, out decimal validado);

            return validado;
        }
        catch (Exception)
        {
            return 0;
        }
    }

    /// <summary>
    /// Valida que tipo de gasto es
    /// </summary>
    /// <returns></returns>
    public static int Validartipo()
    {
        Console.Write("Tipo de gasto: ");
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

    public static int ValidarImpuesto()
    {
        Console.Write("Tipo de impuesto: ");
        int entrada = int.Parse(Console.ReadLine() ?? string.Empty);

        return entrada switch
        {
            4 => (int)tipoImpuesto.IVA4,
            10 => (int)tipoImpuesto.IVA10,
            21 => (int)tipoImpuesto.IVA21,
            15 => (int)tipoImpuesto.IRPF15
        };
    }
}