using Domain.Entity;
using Domain.Validation;
using NuGet.Frameworks;

namespace Tests;

public class TestValidacoes
{
    [Fact]
    public void TestarValidacoesCPF()
    {
        Assert.False(Validacoes.ValidarCPF(null));
        Assert.False(Validacoes.ValidarCPF(string.Empty));
        Assert.False(Validacoes.ValidarCPF("123"));
        Assert.False(Validacoes.ValidarCPF("12345678901"));
        Assert.True(Validacoes.ValidarCPF("123.456.789-09"));
    }

    [Fact]
    public void TestarValidacoesEMail()
    {
        Assert.False(Validacoes.ValidarEMail(null));
        Assert.False(Validacoes.ValidarEMail(string.Empty));
        Assert.False(Validacoes.ValidarEMail("email"));
        Assert.False(Validacoes.ValidarEMail("x@x"));
        Assert.True(Validacoes.ValidarEMail("x@x.com"));
    }

    [Fact]
    public void TestarValidacoesPessoa()
    {
        Pessoa pessoa = new();
        Assert.ThrowsAny<Exception>(() => pessoa.Validar());

        pessoa = new() { CPF = "123.456.789-09", EMail = "x@x.com", Nome = "Teste" };
        pessoa.Validar();
    }

    [Fact]
    public void TestarValidacoesHorarioMedico()
    {
        HorarioMedico horarioMedico = new HorarioMedico(DayOfWeek.Monday, 8, 18);
        horarioMedico.Validar();

        horarioMedico = new HorarioMedico(DayOfWeek.Monday, 18, 8);
        Assert.ThrowsAny<Exception>(() => horarioMedico.Validar());
    }
}