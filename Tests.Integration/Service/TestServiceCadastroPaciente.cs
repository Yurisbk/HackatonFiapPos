using Domain.Interfaces.Service;
using Microsoft.Extensions.DependencyInjection;
using Service.Helper;
using Tests.Integration.Fixture;
using Tests.Integration.Helper;

namespace Tests.Integration.Service;

public class TestServiceCadastroPaciente(WebAppFixture webAppFixture) : TestBaseWebApp(webAppFixture)
{
    IServiceCadastroPaciente serviceCadastroPaciente => ServiceProvider.GetService<IServiceCadastroPaciente>()!;
    HelperTransacao helperTransacao => ServiceProvider.GetService<HelperTransacao>()!;

    [Fact]
    public async Task TestGravarPaciente()
    {
        using (var transacao = helperTransacao.CriaTransacao())
        {
            var paciente = HelperGeracaoEntidades.CriaPacienteValido();

            // Testa gravação (adição/edição)

            await serviceCadastroPaciente.GravarPaciente(paciente);

            paciente.Nome = "Novo Nome";

            await serviceCadastroPaciente.GravarPaciente(paciente);

            // Testa resgate por email

            var pacienteCadastro = await serviceCadastroPaciente.ResgatarPacientePorEmail(paciente.EMail!);

            Assert.Equal(paciente.Nome, pacienteCadastro?.Nome);

            // Testa exclusão

            await serviceCadastroPaciente.ExcluirPaciente(paciente.Id!.Value);

            pacienteCadastro = await serviceCadastroPaciente.ResgatarPacientePorEmail(paciente.EMail!);

            Assert.Null(pacienteCadastro);
        }
    }
}
