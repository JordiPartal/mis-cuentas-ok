using MisCuentas.Domain.Interface;
using MisCuentas.Domain.Models;

namespace MisCuentas.Infrastructure.Tmp.Controller;

public class SumatorioController
{
    private readonly ISumatorioService _sumatorioService;
    
    public SumatorioController(ISumatorioService sumatorioService)
    {
        _sumatorioService = sumatorioService;
    }
    
    public List<Sumatorio> ObtenerSumatorio(int? mes, int? ano, int? concepto)
    {
        return _sumatorioService.ObtenerSumatorio(mes, ano, concepto);
    }
}