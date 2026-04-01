using Microsoft.Extensions.DependencyInjection;
using MisCuentas.Infrastructure.Tmp.Controller;
using MisCuentas.Infrastructure.Tmp.MenuCommand;
using MisCuentas.Domain.Interface;
using MisCuentas.Infrastructure.Service;

namespace MisCuentas.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AnadirController(this IServiceCollection services)
    {
        // servicios
        services.AddTransient<ITransaccionService, TransaccionServices>();
        services.AddTransient<ISumatorioService, SumatorioServices>();
        services.AddTransient<IBalanceService, BalanceService>();
        services.AddTransient<IMovimientoService, MovimientoService>();
        services.AddTransient<IRentabilidadService, RentabilidadService>();
        
        // commands
        services.AddTransient<IMenuCommand, TransaccionCommand>();
        services.AddTransient<IMenuCommand, InsertaTransaccionCommand>();
        services.AddTransient<IMenuCommand, SumatorioCommand>();
        services.AddTransient<IMenuCommand, BalanceCommand>();
        services.AddTransient<IMenuCommand, MovimientoCommand>();
        services.AddTransient<IMenuCommand, RentabilidadCommand>();
        
        // controllers
        services.AddTransient<MenuController>();
        
        return services;
    }
}