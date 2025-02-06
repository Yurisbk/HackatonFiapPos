using contatos_testes_integration.fixture;
using Domain.Entity;
using Domain.Interfaces.Repository;
using Microsoft.Extensions.DependencyInjection;
using Tests.Integration.Helper;

namespace Tests.Integration.Infrastructure;

public class TestRepositoryConsulta(WebAppFixture webAppFixture) : TestBaseWebApp(webAppFixture)
{
    IRepositoryConsulta repositoryConsulta => ServiceProvider.GetService<IRepositoryConsulta>()!;
    IRepositoryMedico repositoryMedico => ServiceProvider.GetService<IRepositoryMedico>()!;

    [Fact]
    public async Task TestCadastroHorarioMedico()
    {
        using (var transacao = CriaTransacao())
        {
            Medico medico = HelperGeracaoEntidades.CriaMedicoValido()!;
            await repositoryMedico.RegistarNovoMedico(medico);
        }
    }
}
