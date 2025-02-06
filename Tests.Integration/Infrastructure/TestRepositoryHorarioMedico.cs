using contatos_testes_integration.fixture;
using Domain.Entity;
using Domain.Interfaces.Repository;
using Microsoft.Extensions.DependencyInjection;
using Service.Helper;
using Tests.Integration.Helper;

namespace Tests.Integration.Infrastructure;

public class TestRepositoryHorarioMedico(WebAppFixture webAppFixture) : TestBaseWebApp(webAppFixture)
{
    IRepositoryHorarioMedico repositoryHorarioMedico => ServiceProvider.GetService<IRepositoryHorarioMedico>()!;
    IRepositoryMedico repositoryMedico => ServiceProvider.GetService<IRepositoryMedico>()!;
    HelperTransacao helperTransacao => ServiceProvider.GetService<HelperTransacao>()!;


    [Fact]
    public async Task TestCadastroHorarioMedico()
    {
        using (var transacao = helperTransacao.CriaTransacao())
        {
            Medico medico = HelperGeracaoEntidades.CriaMedicoValido()!;
            await repositoryMedico.RegistarNovoMedico(medico);

            await Assert.ThrowsAsync<InvalidOperationException>(() => repositoryHorarioMedico.RegistrarHorariosMedicoDiaSemana(-1, DayOfWeek.Monday, []));

            await Assert.ThrowsAsync<ArgumentException>(() => repositoryHorarioMedico.RegistrarHorariosMedicoDiaSemana(medico.Id!.Value, DayOfWeek.Monday, new Periodo(8, 12), new Periodo(11, 14)));

            await repositoryHorarioMedico.RegistrarHorariosMedicoDiaSemana(medico.Id!.Value, DayOfWeek.Monday, new Periodo(8, 12), new Periodo(13, 14));

            var horarios = await repositoryHorarioMedico.ListarHorariosMedicoDiaSemana(DayOfWeek.Monday);

            Assert.Equal(2, horarios.Length);
        }
    }
}
