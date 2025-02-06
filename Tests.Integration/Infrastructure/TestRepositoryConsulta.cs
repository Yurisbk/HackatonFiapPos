using contatos_testes_integration.fixture;
using Domain.Entity;
using Domain.Interfaces.Repository;
using Microsoft.Extensions.DependencyInjection;
using Service.Helper;
using Tests.Integration.Helper;

namespace Tests.Integration.Infrastructure;

public class TestRepositoryConsulta(WebAppFixture webAppFixture) : TestBaseWebApp(webAppFixture)
{
    IRepositoryConsulta repositoryConsulta => ServiceProvider.GetService<IRepositoryConsulta>()!;
    IRepositoryMedico repositoryMedico => ServiceProvider.GetService<IRepositoryMedico>()!;
    HelperTransacao helperTransacao => ServiceProvider.GetService<HelperTransacao>()!;

    [Fact]
    public async Task TestCadastroHorarioMedico()
    {
        using (var transacao = helperTransacao.CriaTransacao())
        {
            Medico medico = HelperGeracaoEntidades.CriaMedicoValido()!;
            await repositoryMedico.RegistarNovoMedico(medico);
        }
    }
}
