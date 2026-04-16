using Microsoft.Extensions.DependencyInjection;
using MisCuentas.Infrastructure.Tmp.Controller;
using MisCuentas.Infrastructure.Tmp.MenuCommand;
using MisCuentas.Domain.Interface;
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
        
        // services
        services.AddTransient<IValidacionService, ValidacionService>();
        services.AddTransient<ICsvService, CsvServices>();
        services.AddTransient<IGestorDeErroresService, GestorDeErroresService>();
        services.AddTransient<ITransaccionService, TransaccionServices>();
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