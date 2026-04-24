using MisCuentas.Domain.Models;

namespace MisCuentas.Domain.Interface.Repository;

public interface ISumatorioRepository
{
    public List<Sumatorio> ObtenerSumatorio(int? mes, int? ano, int? concepto);
}