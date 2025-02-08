using Domain.Interfaces;
using Domain.Validation;

namespace Domain.Entity;

public class Pessoa : Entidade, IValidavel
{
    public string? Nome { get; set; }
    public string? CPF { get; set; }
    public string? EMail { get; set; }

    public virtual void Validar()
    {
        if (!Validacoes.ValidarNome(Nome))
            throw new Exception("Nome inválido");

        if (!Validacoes.ValidarCPF(CPF))
            throw new Exception("CPF inválido");

        if (!Validacoes.ValidarEMail(EMail))
            throw new Exception("E-Mail inválido");
    }
}
