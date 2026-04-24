using MisCuentas.Domain.Models;

namespace MisCuentas.Domain.Interface.Service;

public interface IBalanceService
{
    public Balance ObtenerBalance(int? mes, int? ano);
    public Task<Balance> ObtenerComparativaMesActualAnterior(int mes, int ano);
}