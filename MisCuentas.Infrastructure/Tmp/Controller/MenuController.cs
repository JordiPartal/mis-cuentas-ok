using MisCuentas.Domain.Interface;
using MisCuentas.Domain.Models;
using MisCuentas.Infrastructure.Tmp.Utils;

namespace MisCuentas.Infrastructure.Tmp.Controller;

public class MenuController
{
    private readonly ExportarConfig _exportarConfig;
    private readonly List<IMenuCommand> _commands;

    public MenuController(IEnumerable<IMenuCommand> commands, ExportarConfig exportarConfig)
    {
        _commands = commands.ToList();
        _exportarConfig = exportarConfig;
    }       

    public void Inicio()
    {
        while (true)
        {
            for (int indice = 0; indice < _commands.Count; indice++) Console.WriteLine($"({indice + 1}) {_commands[indice].Nombre}");
            
            Console.WriteLine("(0) Salir");
            Console.WriteLine();
            Console.Write("Ingrese una opcion: ");

            var input = Console.ReadLine();
            if (input.Contains("-e")) _exportarConfig.Exportar = true;
            if (input.Contains("-n")) _exportarConfig.NombreFichero = input.Split(" ").Last();
            
            input = input.Split(" ")[0];
            
            Console.WriteLine();

            if (input == "0") break;
            if (int.TryParse(input, out int opcion) && opcion > 0 && opcion <= _commands.Count) _commands[opcion - 1].Ejecutar();
            else Console.WriteLine("Opcion invalida"); Console.WriteLine();
        }
    }
}