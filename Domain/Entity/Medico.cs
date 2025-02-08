namespace Domain.Entity;

public class Medico : Pessoa
{
    public string? CRM { get; set; }
    public string? Especialidade { get; set; }
    public double ValorConsulta { get; set; }

    public override void Validar()
    {
        base.Validar();

        if (string.IsNullOrWhiteSpace(CRM))
            throw new Exception("CRM inválido");

        if (string.IsNullOrEmpty(Especialidade))
            throw new Exception("Especialidade inválida");

        if (ValorConsulta <= 0)
            throw new Exception("Valor da consulta inválido");
    }
}
