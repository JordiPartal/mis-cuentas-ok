using MisCuentas.Domain.Models;

namespace MisCuentas.Domain.Interface;

public interface IBalanceRepository
{
    public Balance Balance(int? mes, int? ano);
}