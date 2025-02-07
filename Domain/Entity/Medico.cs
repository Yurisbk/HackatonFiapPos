namespace Domain.Entity;

public class Medico : Usuario
{
    public string? CRM;
    public string? Especialidade;
    public double ValorConsulta;

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
