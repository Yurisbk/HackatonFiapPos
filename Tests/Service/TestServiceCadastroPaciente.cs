using Domain.Interfaces.Service;
using Tests.Helper;

namespace Tests.Service;

public class TestServiceCadastroPaciente : TestServiceBase
{
    [Fact]
    public async Task TestGravarPaciente()
    {
        using (var transacao = TransacaoFactory.CriaTransacao())
        {
            var paciente = HelperGeracaoEntidades.CriaPacienteValido();

            // Testa gravação (adição/edição)

            await ServiceCadastroPaciente.GravarPaciente(paciente);

            paciente.Nome = "Novo Nome";

            await ServiceCadastroPaciente.GravarPaciente(paciente);

            // Testa resgate por email

            var pacienteCadastro = await ServiceCadastroPaciente.ResgatarPacientePorEmail(paciente.EMail!);

            Assert.Equal(paciente.Nome, pacienteCadastro?.Nome);

            // Testa exclusão

            await ServiceCadastroPaciente.ExcluirPaciente(paciente.Id!.Value);

            pacienteCadastro = await ServiceCadastroPaciente.ResgatarPacientePorEmail(paciente.EMail!);

            Assert.Null(pacienteCadastro);
        }
    }
}
