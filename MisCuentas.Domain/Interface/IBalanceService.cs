using MisCuentas.Domain.Models;

namespace MisCuentas.Domain.Interface;

public interface IBalanceService
{
    public Balance Balance(int? mes, int? ano);
}