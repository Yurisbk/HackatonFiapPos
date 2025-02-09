using Domain.Entity;
using Tests.Helper;

namespace Tests.Service;

public class TestServiceCadastroMedico: TestServiceBase
{    
    [Fact]
    public async Task TestCadastroMedico()
    {
        using (var transacao = TransacaoFactory.CriaTransacao())
        {
            var medico = HelperGeracaoEntidades.CriaMedicoValido();

            // Testa gravação (adição/edição)

            await ServiceCadastroMedico.GravarMedico(medico);

            medico.Nome = "Novo Nome";

            await ServiceCadastroMedico.GravarMedico(medico);

            // Testa resgate por CRM

            var medicoCadastro = await ServiceCadastroMedico.ResgatarMedicoPorCRM(medico.CRM!);

            Assert.Equal(medico.Nome, medicoCadastro?.Nome);

            // Testa exclusão

            await ServiceCadastroMedico.ExcluirMedico(medico.Id!.Value);

            medicoCadastro = await ServiceCadastroMedico.ResgatarMedicoPorCRM(medico.CRM!);

            Assert.Null(medicoCadastro);
        }
    }

    [Fact]
    public async Task TestCadastroMedicoEspecialidades()
    {
        using (var transacao = TransacaoFactory.CriaTransacao())
        {
            const string especialidadeTeste = "especialidade teste";

            for (int i = 0; i < 5; i++)
            {
                var medico = HelperGeracaoEntidades.CriaMedicoValido();
                medico.Especialidade = especialidadeTeste;
                await ServiceCadastroMedico.GravarMedico(medico);
                await ServiceHorarioMedico.RegistrarHorariosMedicoDiaSemana(medico.Id!.Value, DayOfWeek.Monday, [new Periodo(8, 18)]);
            }

            const string especialidadeTeste2 = "especialidade teste2";

            for (int i = 0; i < 3; i++)
            {
                var medico = HelperGeracaoEntidades.CriaMedicoValido();
                medico.Especialidade = especialidadeTeste2;
                await ServiceCadastroMedico.GravarMedico(medico);
                await ServiceHorarioMedico.RegistrarHorariosMedicoDiaSemana(medico.Id!.Value, DayOfWeek.Monday, [new Periodo(8, 18)]);
            }

            var especialidades = await ServiceCadastroMedico.ListarEspecialidadeMedicas();
            Assert.True(especialidades.Length >= 2);

            var medicos = await ServiceCadastroMedico.ListarMedicosAtivosNaEspecialidade(especialidadeTeste);
            Assert.Equal(5, medicos.Length);

            medicos = await ServiceCadastroMedico.ListarMedicosAtivosNaEspecialidade(especialidadeTeste2);
            Assert.Equal(3, medicos.Length);
        }
    }
}
