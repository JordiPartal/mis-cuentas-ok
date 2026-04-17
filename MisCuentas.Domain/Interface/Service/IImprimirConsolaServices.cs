using MisCuentas.Domain.Models;

namespace MisCuentas.Domain.Interface.Service;

public interface IImprimirConsolaServices
{
    public void Bienvenida();
    public void Transacciones(List<Transaccion> transacciones);
    public void Balance(Balance balance);
    public void Sumatorios(List<Sumatorio> sumatorios);
    public void ImprimirTipoDeGasto();
    public void ImprimirTipoDeImpuesto();
}