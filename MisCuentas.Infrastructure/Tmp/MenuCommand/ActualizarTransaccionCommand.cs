using MisCuentas.Domain.Interface;
using MisCuentas.Domain.Models;
using MisCuentas.Infrastructure.Tmp.Utils;

namespace MisCuentas.Infrastructure.Tmp.MenuCommand;

public class InsertaTransaccionCommand : IMenuCommand
{
    private readonly ITransaccionService _transaccionService;
    public string Nombre => "Añadir Transacciones";
    
    public InsertaTransaccionCommand(ITransaccionService transaccionService)
    {
        _transaccionService = transaccionService;
    }
    
    public void Ejecutar()
    {
        var transaccion = new Transaccion();
        
        transaccion.fechaCargo = Validacion.ValidarFecha();
        transaccion.concepto = Validacion.ValidarString("Concepto: ");
        transaccion.cantidad = Validacion.ValidarNumero();
        
        ImpresoraDeConsola.Imprimirtipo();
        transaccion.idtipo = Validacion.Validartipo();
        
        ImpresoraDeConsola.ImprimirImpuesto();
        transaccion.idImpuesto = Validacion.ValidarImpuesto();
        
        _transaccionService.AgregarTransaccion(transaccion);
    }
}