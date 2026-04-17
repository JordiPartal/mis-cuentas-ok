namespace MisCuentas.Domain.Interface.Service;

public interface ICsvService
{
    void ExportarCSV<T>(List<T> data, string nombreArchivo);
}