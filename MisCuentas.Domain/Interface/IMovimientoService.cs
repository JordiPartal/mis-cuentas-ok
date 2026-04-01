using MisCuentas.Domain.Models;

namespace MisCuentas.Domain.Interface;

public interface IMovimientoService
{
    public void ExportarCSV(int? mes, int? ano);
}