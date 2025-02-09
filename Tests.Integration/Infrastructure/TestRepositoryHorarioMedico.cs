using Domain.Entity;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Service;
using Microsoft.Extensions.DependencyInjection;
using Tests.Helper;
using Tests.Integration.Fixture;

namespace Tests.Integration.Infrastructure;

public class TestRepositoryHorarioMedico(WebAppFixture webAppFixture) : TestBaseWebApp(webAppFixture)
{
    IRepositoryHorarioMedico repositoryHorarioMedico => ServiceProvider.GetService<IRepositoryHorarioMedico>()!;
    IRepositoryMedico repositoryMedico => ServiceProvider.GetService<IRepositoryMedico>()!;
    ITransacaoFactory transacaoFactory => ServiceProvider.GetService<ITransacaoFactory>()!;

    [Fact]
    public async Task TestCadastroHorarioMedico()
    {
        using (var transacao = transacaoFactory.CriaTransacao())
        {
            Medico medico = HelperGeracaoEntidades.CriaMedicoValido()!;
            await repositoryMedico.RegistarNovoMedico(medico);

            // Testa horarios invalidos

            await Assert.ThrowsAsync<InvalidOperationException>(() => repositoryHorarioMedico.RegistrarHorariosMedicoDiaSemana(-1, DayOfWeek.Monday, []));
            await Assert.ThrowsAsync<ArgumentException>(() => repositoryHorarioMedico.RegistrarHorariosMedicoDiaSemana(medico.Id!.Value, DayOfWeek.Monday, new Periodo(8, 12), new Periodo(11, 14)));

            // Testa e assegura gravaão de horarios validos

            await repositoryHorarioMedico.RegistrarHorariosMedicoDiaSemana(medico.Id!.Value, DayOfWeek.Monday, new Periodo(8, 12), new Periodo(13, 14));

            var horarios = await repositoryHorarioMedico.ResgatarHorariosMedicoDiaSemana(medico.Id!.Value, DayOfWeek.Monday);

            Assert.True(horarios.Length == 2);
        }
    }
}
