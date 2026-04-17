using System.Globalization;
using MisCuentas.Domain.Interface.Service;
using MisCuentas.Domain.Models;

namespace MisCuentas.Infrastructure.Service;

public class ImprimirConsolaService : IImprimirConsolaServices
{
    /// <summary>
    /// Adjusts the length of a string by adding padding spaces to the right
    /// until the desired size is reached. If the specified size is zero,
    /// the string is adjusted to a default length of 24 characters.
    /// </summary>
    /// <param name="nombre">The original string to be adjusted.</param>
    /// <param name="tamano">The desired length for the adjusted string. If zero, a default size of 24 is applied.</param>
    /// <returns>Returns the string adjusted to the specified length with spaces padded to the right.</returns>
    private static string Tamano(string nombre, int tamano)
    {
        if (tamano == 0) return nombre.PadRight(24, ' ');
        return nombre.PadRight(tamano, ' ');
    }
    
    /// <summary>
    /// Displays a welcome message to the user in the console.
    /// This method is used to introduce the user to the application
    /// by printing a formatted message with decorative elements.
    /// </summary>
    public void Bienvenida()
    {
        Console.WriteLine(new string('=', 48));
        Console.WriteLine("> Bienvenido al gestor de cuentas".ToUpper());
        Console.WriteLine(new string('=', 48));
        Console.WriteLine();
    }

    /// <summary>
    /// Displays a formatted table of transactions in the console,
    /// including details such as date, type, concept, base amount,
    /// tax amount, and total amount. If no transactions are provided,
    /// an error message is displayed.
    /// </summary>
    /// <param name="transacciones">A list of transaction objects containing the details to be displayed in the table.</param>
    /// <exception cref="Exception">Thrown when the list of transactions is empty.</exception>
    public void Transacciones(List<Transaccion> transacciones)
    {
        try
        {
            if (transacciones.Count == 0) throw new Exception();
            
            string[] cabecera = { Tamano("cargo", 23), Tamano("tipo", 23), Tamano("concepto", 23), Tamano("sin impuesto", 23), Tamano("iva/irpf", 23), Tamano("total", 0) };
            string[] separado = { new string('*', 23), new string('*', 23), new string('*', 23), new string('*', 23), new string('*', 23), new string('*', 24) };

            Console.WriteLine(string.Join("|", cabecera));
            Console.WriteLine(string.Join("|", separado));

            foreach (var item in transacciones)
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
    }

    /// <summary>
    /// Displays the financial balance in the console by formatting and outputting
    /// the details of income, expenses, savings, and profit in a structured manner.
    /// </summary>
    /// <param name="balance">The financial balance object containing details about income, expenses, savings, and profit percentages.</param>
    /// <exception cref="Exception">Thrown if the provided balance object is null or cannot be processed.</exception>
    public void Balance(Balance balance)
    {
        try
        {
            if (balance == null) throw new Exception();

            CultureInfo culture = CultureInfo.InvariantCulture;
            string ingreso, gasto, ahorro, ganancia;
            string[] cabecera = { Tamano("ingresos", 23), Tamano("gastos", 23), Tamano("ahorro", 23), Tamano("ganancia", 0) };
            string[] separado = { new('*', 23), new ('*', 23), new ('*', 23), new('*', 23) };

            Console.WriteLine(string.Join("|", cabecera));
            Console.WriteLine(string.Join("|", separado));

            ingreso = balance.ingresos.ToString("C").PadLeft(23, ' ');
            gasto = balance.gastos.ToString("C").PadLeft(23, ' ');
            ahorro = balance.ahorro.ToString("C").PadLeft(23, ' ');
            ganancia = string.Concat(balance.ganancia.ToString("N", culture), "%").PadLeft(23, ' ');

            Console.WriteLine($"{ingreso}|{gasto}|{ahorro}|{ganancia}");

            Console.WriteLine();
        }
        catch (Exception)
        {
            Console.WriteLine("* N/D *");
        }
    }

    /// <summary>
    /// Displays a summary table of items in the console,
    /// showing the concept and total amount for each entry in the list.
    /// </summary>
    /// <param name="lista">A list of <see cref="Sumatorio"/> objects containing the concept and total amount to be displayed in the summary table.</param>
    /// <exception cref="Exception">Thrown when the list of items is empty.</exception>
    public void Sumatorios(List<Sumatorio> lista)
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
    /// Displays the available types of expenses in the console, providing a list
    /// of categories such as Nomina, Simple, Renting, Transporte, Ocio, and Ingreso.
    /// If a type is not specified or found, the default type "Simple" will be assigned.
    /// This method is intended to inform the user about the predefined expense categories.
    /// </summary>
    public void ImprimirTipoDeGasto()
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
    /// Displays the different types of taxes in the console along with their corresponding values.
    /// This method lists various tax options, such as VAT with different percentages and IRPF,
    /// and includes a note indicating the default tax of 0% if none is applied.
    /// </summary>
    public void ImprimirTipoDeImpuesto()
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