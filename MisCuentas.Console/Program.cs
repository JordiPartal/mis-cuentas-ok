using Microsoft.Extensions.DependencyInjection;
using MisCuentas.Infrastructure;
using MisCuentas.Infrastructure.Tmp.Controller;
using MisCuentas.Infrastructure.Tmp.Utils;

var service = new ServiceCollection();
service.AnadirController();

var menu = service.BuildServiceProvider();

ImpresoraDeConsola.ImprimirBienvenida();
menu.GetRequiredService<MenuController>().Inicio();