using contatos_testes_integration.fixture;
using Domain.Interfaces.Repository;
using Microsoft.Extensions.DependencyInjection;
using Service.Helper;
using Tests.Integration.Helper;

namespace Tests.Integration.Infrastructure;

public class TestRepositoryMedico(WebAppFixture webAppFixture) : TestBaseWebApp(webAppFixture)
{
    IRepositoryMedico repositoryMedico => ServiceProvider.GetService<IRepositoryMedico>()!;
    HelperTransacao helperTransacao => ServiceProvider.GetService<HelperTransacao>()!;

    [Fact]
    public async Task TestCadastroMedico()
    {
        using (var transacao = helperTransacao.CriaTransacao())
        {
            // Testa registro

            var medico = HelperGeracaoEntidades.CriaMedicoValido();
            await repositoryMedico.RegistarNovoMedico(medico);

            // Testa Uks

            var outroMedico = HelperGeracaoEntidades.CriaMedicoValido();
            outroMedico.CPF = medico.CPF;
            await Assert.ThrowsAsync<InvalidOperationException>(() => repositoryMedico.RegistarNovoMedico(medico));

            outroMedico = HelperGeracaoEntidades.CriaMedicoValido();
            outroMedico.EMail = medico.EMail;
            await Assert.ThrowsAsync<InvalidOperationException>(() => repositoryMedico.RegistarNovoMedico(medico));

            outroMedico = HelperGeracaoEntidades.CriaMedicoValido();
            outroMedico.CRM = medico.CRM;
            await Assert.ThrowsAsync<InvalidOperationException>(() => repositoryMedico.RegistarNovoMedico(medico));

            // Testa resgates

            var medicoCadastro = await repositoryMedico.ResgatarMedicoPorId(medico.Id!.Value);
            Assert.Equal(medico.CPF, medicoCadastro?.CPF);

            medicoCadastro = await repositoryMedico.ResgatarMedicoPorEmail(medico.EMail!);
            Assert.Equal(medico.EMail, medicoCadastro?.EMail);

            // Testa alteração

            medico.Nome = "Novo Nome";
            await repositoryMedico.AlterarDadosMedico(medico);

            medicoCadastro = await repositoryMedico.ResgatarMedicoPorId(medico.Id!.Value);
            Assert.Equal(medico.Nome, medicoCadastro?.Nome);

            outroMedico = HelperGeracaoEntidades.CriaMedicoValido();
            await repositoryMedico.RegistarNovoMedico(outroMedico);

            // Testa UK em alteração

            outroMedico.CPF = medico.CPF;
            await Assert.ThrowsAsync<InvalidOperationException>(() => repositoryMedico.AlterarDadosMedico(outroMedico));

            // Testa exclusão

            await repositoryMedico.ExcluirMedico(medico.Id!.Value);
            medicoCadastro = await repositoryMedico.ResgatarMedicoPorId(medico.Id!.Value);
            Assert.Null(medicoCadastro);
        }
    }
}
