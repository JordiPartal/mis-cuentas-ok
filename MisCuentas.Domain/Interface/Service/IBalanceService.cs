using MisCuentas.Domain.Models;

namespace MisCuentas.Domain.Interface.Service;

public interface IBalanceService
{
    public Balance ObtenerBalance(int? mes, int? ano);
}