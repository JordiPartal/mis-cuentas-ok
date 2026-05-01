using Microsoft.Extensions.DependencyInjection;
using MisCuentas.Infrastructure;
using MisCuentas.Infrastructure.Service;
using MisCuentas.Presentation;

var service = new ServiceCollection();
service.AnadirController();

var menu = service.BuildServiceProvider();

menu.GetRequiredService<ImprimirConsolaService>().Bienvenida();
menu.GetRequiredService<MenuController>().Inicio();