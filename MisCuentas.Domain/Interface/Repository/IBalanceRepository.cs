using MisCuentas.Domain.Models;

namespace MisCuentas.Domain.Interface.Repository;

public interface IBalanceRepository
{
    public Task<Balance> BalanceAsync(int? mes, int? ano);
    public Balance Balance(int? mes, int? ano);
}