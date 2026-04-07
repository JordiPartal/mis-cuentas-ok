using System.Globalization;
using MisCuentas.Domain.Models;

namespace MisCuentas.Infrastructure.Tmp.Utils;

public class ImpresoraDeConsola
{
    /// <summary>
    /// Ajusta el tamaño del texto proporcionando un nombre con un relleno basado en el tamaño especificado.
    /// </summary>
    /// <param name="nombre">El texto original que se desea ajustar.</param>
    /// <param name="tamano">La longitud deseada para el texto ajustado. Si es 0, se usa una longitud predeterminada de 24 caracteres.</param>
    /// <returns>El texto ajustado con el relleno especificado.</returns>
    private static string Tamano(string nombre, int tamano)
    {
        if (tamano == 0) return nombre.PadRight(24, ' ');
        return nombre.PadRight(tamano, ' ');
    }

    /// <summary>
    /// Imprime un mensaje de bienvenida en la consola, formateado con líneas de separación
    /// y el título en mayúsculas.
    /// </summary>
    public static void ImprimirBienvenida()
    {
        string titulo = "> Bienvenido al gestor de cuenta";
        
        Console.WriteLine();
        
        Console.WriteLine(new string('=', 48));
        Console.WriteLine(titulo.ToUpper());
        Console.WriteLine(new string('=', 48));
        
        Console.WriteLine();
    }

    /// <summary>
    /// Imprime en consola una lista de transacciones con su información detallada,
    /// incluyendo fecha, tipo, concepto y cantidad en un formato tabular.
    /// </summary>
    /// <param name="lista">Lista de transacciones a imprimir. Debe contener al menos un elemento; de lo contrario, se muestra un mensaje de error.</param>
    /// <exception cref="Exception">Se lanza una excepción si la lista está vacía.</exception>
    public static void ImprimirTransaccion(List<Transaccion> lista)
    {
        try
        {
            if (lista.Count == 0) throw new Exception();
            
            string[] cabecera = { Tamano("cargo", 23), Tamano("tipo", 23), Tamano("concepto", 23), Tamano("sin impuesto", 23), Tamano("iva/irpf", 23), Tamano("total", 0) };
            string[] separado = { new string('*', 23), new string('*', 23), new string('*', 23), new string('*', 23), new string('*', 23), new string('*', 24) };

            Console.WriteLine(string.Join("|", cabecera));
            Console.WriteLine(string.Join("|", separado));

            foreach (var item in lista)
            {
                string fecha = item.fechaCargo.ToString("ddd, dd \\de MMM yyyy").PadRight(23, ' ');
                string tipo = item.tipo.PadRight(23, ' ');
                string concepto = item.concepto.PadRight(23, ' ');
                string _base = item._base.ToString("C").PadLeft(23, ' ');
                string cuota = item.cuota.ToString("C").PadLeft(23, ' ');
                string cantidad = item.cantidad.ToString("C").PadLeft(24, ' ');

                Console.WriteLine($"{string.Join("|", fecha, tipo, concepto, _base, cuota, cantidad)}");
            }
        }
        catch (Exception)
        {
            Console.WriteLine("* N/D *");
        }
        
        Console.WriteLine();
    }

    /// <summary>
    /// Imprime un resumen de sumatorios con formato estructurado en la consola.
    /// </summary>
    /// <param name="lista">Lista de objetos de tipo Sumatorio que se desea imprimir.</param>
    /// <exception cref="Exception">Se lanza una excepción si la lista está vacía.</exception>
    public static void ImprimirSumatorio(List<Sumatorio> lista)
    {
        try
        {
            if (lista.Count == 0) throw new Exception();
            
            string[] cabecera = { Tamano("concepto", 23), "|", Tamano("total", 0) };
            string[] separado = { new string('*', 23), "|", new string('*', 24) };

            Console.WriteLine(string.Join("", cabecera));
            Console.WriteLine(string.Join("", separado));

            foreach (var item in lista)
            {
                string concepto = item.concepto.PadRight(23, ' ');
                string total = item.total.ToString("C").PadLeft(24, ' ');
                Console.WriteLine($"{concepto}|{total}");
            }
        }
        catch (Exception)
        {
            Console.WriteLine("* N/D *");
        }
        
        Console.WriteLine();
    }
    
    /// <summary>
    /// Imprime el balance mensual
    /// </summary>
    /// <param name="Balance"></param>
    public static void ImprimirBalance(Balance Balance)
    {
        try
        {
            if (Balance == null) throw new Exception();

            CultureInfo culture = CultureInfo.InvariantCulture;
            string ingreso, gasto, ahorro, ganancia;
            string[] cabecera = { Tamano("ingresos", 23), Tamano("gastos", 23), Tamano("ahorro", 23), Tamano("ganancia", 0) };
            string[] separado = { new('*', 23), new ('*', 23), new ('*', 23), new('*', 23) };

            Console.WriteLine(string.Join("|", cabecera));
            Console.WriteLine(string.Join("|", separado));

            ingreso = Balance.ingresos.ToString("C").PadLeft(23, ' ');
            gasto = Balance.gastos.ToString("C").PadLeft(23, ' ');
            ahorro = Balance.ahorro.ToString("C").PadLeft(23, ' ');
            ganancia = string.Concat(Balance.ganancia.ToString("N", culture), "%").PadLeft(23, ' ');

            Console.WriteLine($"{ingreso}|{gasto}|{ahorro}|{ganancia}");

            Console.WriteLine();
        }
        catch (Exception)
        {
            Console.WriteLine("* N/D *");
        }
    }

    /// <summary>
    /// Imprime en la consola los diferentes tipos disponibles de transacciones con un formato específico.
    /// Incluye una nota indicando que, si no se encuentra el tipo deseado, se asignará automáticamente como "SIMPLE".
    /// </summary>
    public static void Imprimirtipo()
    {
        Console.WriteLine();
        Console.WriteLine("> Nomina");
        Console.WriteLine("> Simple");
        Console.WriteLine("> Renting");
        Console.WriteLine("> Transporte");
        Console.WriteLine("> Ocio");
        Console.WriteLine("> Ingreso");
        Console.WriteLine();
        Console.WriteLine("* En caso de no encontrar el tipo, se asignará SIMPLE *");
        Console.WriteLine();
    }

    /// <summary>
    /// Imprime en la consola una lista de tipos de impuestos disponibles, cada uno asociado a su porcentaje,
    /// e indica que el 21% será asignado por defecto si no se especifica un tipo válido.
    /// </summary>
    public static void ImprimirImpuesto()
    {
        Console.WriteLine();
        Console.WriteLine("> SIN IVA -> 0");
        Console.WriteLine("> IVA 4% -> 4");
        Console.WriteLine("> IVA 10% -> 10");
        Console.WriteLine("> IVA 21% -> 21");
        Console.WriteLine("> IRPF 15% -> 15");
        Console.WriteLine();
        Console.WriteLine("* En caso no haber impuesto se aplicará el 0% *");
        Console.WriteLine();
    }
}