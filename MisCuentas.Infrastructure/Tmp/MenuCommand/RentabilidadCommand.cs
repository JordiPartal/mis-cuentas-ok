using MisCuentas.Domain.Interface;
using MisCuentas.Domain.Interface.Service;

namespace MisCuentas.Infrastructure.Tmp.MenuCommand;

public class RentabilidadCommand : IMenuCommand
{
    private readonly IRentabilidadService _rentabilidadService;
    
    public string Nombre => "Rentabilidad";
    
    public RentabilidadCommand(IRentabilidadService rentabilidadService)
    {
        _rentabilidadService = rentabilidadService;
    }
    
    public void Ejecutar()
    {
        _rentabilidadService.ExportarCSV();
    }
}