namespace MisCuentas.Domain.Interface;

public interface IMenuCommand
{
    string Nombre { get; }
    void Ejecutar();
}