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
    public async Task TestCadastroMedico()
    {
        using (var transacao = helperTransacao.CriaTransacao())
        {
            var medico = HelperGeracaoEntidades.CriaMedicoValido();

            // Testa gravação (adição/edição)

            await serviceCadastroMedico.GravarMedico(medico);

            medico.Nome = "Novo Nome";

            await serviceCadastroMedico.GravarMedico(medico);

            // Testa resgate por CRM

            var medicoCadastro = await serviceCadastroMedico.ResgatarMedicoPorCRM(medico.CRM!);

            Assert.Equal(medico.Nome, medicoCadastro?.Nome);

            // Testa exclusão

            await serviceCadastroMedico.ExcluirMedico(medico.Id!.Value);

            medicoCadastro = await serviceCadastroMedico.ResgatarMedicoPorCRM(medico.CRM!);

            Assert.Null(medicoCadastro);
        }
    }

    [Fact]
    public async Task TestCadastroMedicoEspecialidades()
    {
        using (var transacao = helperTransacao.CriaTransacao())
        {
            const string especialidadeTeste = "especialidade teste";

            for (int i = 0; i < 5; i++)
            {
                var medico = HelperGeracaoEntidades.CriaMedicoValido();
                medico.Especialidade = especialidadeTeste;
                await serviceCadastroMedico.GravarMedico(medico);
            }

            const string especialidadeTeste2 = "especialidade teste2";

            for (int i = 0; i < 3; i++)
            {
                var medico = HelperGeracaoEntidades.CriaMedicoValido();
                medico.Especialidade = especialidadeTeste2;
                await serviceCadastroMedico.GravarMedico(medico);
            }

            var especialidades = await serviceCadastroMedico.ListarEspecialidadeMedicas();
            Assert.Equal(2, especialidades.Length);

            var medicos = await serviceCadastroMedico.ListarMedicosAtivosNaEspecialidade(especialidadeTeste);
            Assert.Equal(5, medicos.Length);

            medicos = await serviceCadastroMedico.ListarMedicosAtivosNaEspecialidade(especialidadeTeste2);
            Assert.Equal(3, medicos.Length);
        }
    }
}
