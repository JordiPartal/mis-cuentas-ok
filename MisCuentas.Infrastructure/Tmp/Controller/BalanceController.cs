using MisCuentas.Domain.Interface;
using MisCuentas.Domain.Models;

namespace MisCuentas.Infrastructure.Tmp.Controller;

public class BalanceController
{
    private readonly IBalanceService _BalanceService;
    
    public BalanceController(IBalanceService BalanceService)
    {
        _BalanceService = BalanceService;
    }
    
    public Balance ObtenerBalance(int? mes, int? ano)
    {
        return _BalanceService.Balance(mes, ano);
    }
}