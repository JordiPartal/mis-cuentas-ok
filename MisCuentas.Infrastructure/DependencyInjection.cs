using Microsoft.Extensions.DependencyInjection;
using MisCuenta.Infrastructure.Data.Repository;
using MisCuenta.Infrastructure.Service;
using MisCuentas.Infrastructure.Tmp.Controller;
using MisCuentas.Infrastructure.Tmp.MenuCommand;
using MisCuentas.Domain.Interface;
using MisCuentas.Domain.Interface.Repository;
using MisCuentas.Domain.Interface.Service;
using MisCuentas.Domain.Models;
using MisCuentas.Infrastructure.Data;
using MisCuentas.Infrastructure.Data.Repository;
using MisCuentas.Infrastructure.Service;

namespace MisCuentas.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AnadirController(this IServiceCollection services)
    {
        services.AddScoped<EstadoFecha>();
        services.AddScoped<ExportarConfig>();
        services.AddScoped<ConexionBd>();
        
        // repository
        services.AddTransient<ITransaccionRepository, TransaccionRepository>();
        services.AddTransient<ISumatorioRepository, SumatorioRepository>();
        services.AddTransient<IBalanceRepository, BalanceRepository>();
        services.AddTransient<IRentabilidadRepository, RentabilidadRepository>();
        
        // services
        services.AddTransient<IValidacionService, ValidacionService>();
        services.AddTransient<ICsvService, CsvService>();
        services.AddTransient<IGestorDeErroresService, GestorDeErroresService>();
        services.AddTransient<IImprimirConsolaServices, ImprimirConsolaService>();
        services.AddTransient<ITransaccionService, TransaccionServices>();
        services.AddTransient<IMovimientoService, MovimientoService>();
        services.AddTransient<IRentabilidadService, RentabilidadService>();
            
        // commands
        services.AddTransient<IMenuCommand, TransaccionServices>();
        services.AddTransient<IMenuCommand, SumatorioCommand>();
        services.AddTransient<IMenuCommand, BalanceService>();
        services.AddTransient<IMenuCommand, MovimientoCommand>();
        services.AddTransient<IMenuCommand, RentabilidadService>();
        
        // controllers
        services.AddTransient<MenuController>();
        
        return services;
    }
}