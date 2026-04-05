using MisCuentas.Domain.Models;

namespace MisCuentas.Domain.Interface;

public interface ISumatorioRepository
{
    public List<Sumatorio> ObtenerSumatorio(int? mes, int? ano, int? concepto);
    public void ExportarCSV(List<Sumatorio> sumatorios);
}