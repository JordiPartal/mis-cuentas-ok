using System.Globalization;
using MisCuentas.Domain.Enums;
using MisCuentas.Domain.Interface.Service;

namespace MisCuentas.Infrastructure.Service;

public class ValidacionService : IValidacionService
{
    /// <summary>
    /// Validates a numeric input provided by the user through the console.
    /// Displays a message to prompt for input and attempts to parse the response as an integer.
    /// If the parsing is successful, the integer value is returned; otherwise, null is returned.
    /// </summary>
    /// <param name="mensaje">The message to be displayed to the user prompting for input.</param>
    /// <returns>
    /// An integer value if the user input is successfully parsed as a number; otherwise, null.
    /// </returns>
    public int? ValidarNumero(string mensaje)
    {
        Console.Write(mensaje);
        string? entrada = Console.ReadLine();
        return int.TryParse(entrada, out int resultado) ? resultado : null;
    }

    /// <summary>
    /// Validates a text input provided by the user through the console.
    /// Displays a message to prompt for the input, reads the response,
    /// and checks if it is non-empty. Returns the user-provided text if valid;
    /// otherwise, returns an empty string.
    /// </summary>
    /// <param name="mensaje">The message to display to the user prompting for text input.</param>
    /// <returns>
    /// The user-provided text if successfully entered; otherwise, an empty string.
    /// </returns>
    public string ValidarTexto(string mensaje)
    {
        Console.Write(mensaje);
        string? cadena = Console.ReadLine();
        if (cadena.Length > 0) return cadena;
        return string.Empty;
    }

    /// <summary>
    /// Validates a decimal input provided by the user through the console.
    /// Displays a message to prompt for input and attempts to parse the response as a decimal value.
    /// If the parsing is successful, the decimal value is returned; otherwise, 0 is returned.
    /// </summary>
    /// <param name="mensaje">The message to be displayed to the user prompting for input.</param>
    /// <returns>
    /// A decimal value if the user input is successfully parsed as a decimal; otherwise, 0.
    /// </returns>
    public decimal ValidarDecimal(string mensaje)
    {
        Console.Write(mensaje);
        string? entrada = Console.ReadLine();
        return decimal.TryParse(entrada, out decimal resultado) ? resultado : 0;
    }

    /// <summary>
    /// Validates a date input provided by the user through the console.
    /// Prompts the user with a message and attempts to parse the input in a specific date format ("dd-MM-yyyy").
    /// If parsing fails, the current date is returned.
    /// </summary>
    /// <param name="mensaje">The message to be displayed to the user prompting for date input.</param>
    /// <returns>
    /// A DateTime object representing the parsed date if input is valid, or the current date if the input is invalid.
    /// </returns>
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
        
        Console.WriteLine();
        Console.WriteLine("* Formato inválido o no ha introducido fecha, se pondrá la fecha de hoy *");
        Console.WriteLine();
        
        return DateTime.Today; 
    }

    /// <summary>
    /// Validates the type of transaction based on user input from the console.
    /// Displays a message prompting the user to specify a type, parses the input, and maps it to a corresponding transaction type.
    /// </summary>
    /// <param name="mensaje">The message displayed to the user prompting for a transaction type.</param>
    /// <returns>
    /// An integer representing the transaction type if input matches a predefined type; otherwise, the integer representing the default 'Simple' type.
    /// </returns>
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

    /// <summary>
    /// Validates a tax type input provided by the user through the console.
    /// Displays a message to prompt for input, parses the response as an integer,
    /// and maps it to a corresponding tax type. If the input does not match any defined types, a default value is returned.
    /// </summary>
    /// <param name="mensaje">The message to be displayed to the user prompting for input.</param>
    /// <returns>
    /// An integer representing the tax type based on the user's input.
    /// If the input is invalid or does not match, a default tax type is returned.
    /// </returns>
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