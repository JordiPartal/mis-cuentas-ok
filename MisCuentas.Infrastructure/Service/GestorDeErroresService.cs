using MisCuentas.Domain.Interface;
using MySql.Data.MySqlClient;

namespace MisCuentas.Infrastructure.Service;

public class GestorDeErroresService : IGestorDeErroresService
{
    public void GestionarExcepcionesMySQL(MySqlException mySqlException)
    {
        string carpeta = Path.Combine(Directory.GetCurrentDirectory(), "logs");
        string fecha = DateTime.Now.ToString("yyyy-MM-dd") + ".log";
        string rutaCompleta = Path.Combine(carpeta, fecha);
        string mensaje = $"[{DateTime.Now:HH:mm:ss}] Error MySQL: {mySqlException.Message}{Environment.NewLine}";
        
        if(!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);
        
        File.AppendAllText(rutaCompleta, mensaje);
    }
}