using MySql.Data.MySqlClient;

namespace MisCuentas.Domain.Interface.Service;

public interface IGestorDeErroresService
{
    public void GestionarExcepcionesMySQL(MySqlException exception);
}