using Domain.Interfaces.Service;
using Microsoft.Extensions.DependencyInjection;
using Service.Helper;
using Tests.Integration.Fixture;
using Tests.Integration.Helper;

namespace Tests.Integration.Service;

public class TestServiceCadastroMedico(WebAppFixture webAppFixture) : TestBaseWebApp(webAppFixture)
{
    IServiceCadastroMedico serviceCadastroMedico => ServiceProvider.GetService<IServiceCadastroMedico>()!;
    HelperTransacao helperTransacao => ServiceProvider.GetService<HelperTransacao>()!; 

    [Fact]
    public async Task TestGravarMedico()
    {
        using (var transacao = helperTransacao.CriaTransacao())
        {
            var medico = HelperGeracaoEntidades.CriaMedicoValido();

            // Testa gravação (adição/edição)

            await serviceCadastroMedico.GravarMedico(medico);

            medico.Nome = "Novo Nome";

            await serviceCadastroMedico.GravarMedico(medico);

            // Testa resgate por email

            var medicoCadastro = await serviceCadastroMedico.ResgatarMedicoPorEmail(medico.EMail!);

            Assert.Equal(medico.Nome, medicoCadastro?.Nome);

            // Testa exclusão

            await serviceCadastroMedico.ExcluirMedico(medico.Id!.Value);

            medicoCadastro = await serviceCadastroMedico.ResgatarMedicoPorEmail(medico.EMail!);

            Assert.Null(medicoCadastro);
        }
    }
}
