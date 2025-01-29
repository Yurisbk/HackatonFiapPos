using Domain.Entidades;

namespace Medico.Domain.Entitidades;

public class Medico: Usuario
{
    public string? CRM;

    public override void Validar()
    {
        base.Validar();

        if (string.IsNullOrWhiteSpace(CRM))
            throw new Exception("CRM inválido");
    }
}
