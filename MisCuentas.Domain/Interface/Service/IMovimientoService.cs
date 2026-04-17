namespace MisCuentas.Domain.Interface.Service;

public interface IMovimientoService
{
    public void ExportarCSV(int? mes, int? ano);
}