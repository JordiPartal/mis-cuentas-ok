namespace MisCuentas.Domain.Interface;

public interface ICsvService
{
    void ExportarCSV<T>(List<T> data, string nombreArchivo);
}