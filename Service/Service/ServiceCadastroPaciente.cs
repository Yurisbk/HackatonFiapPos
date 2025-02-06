using Domain.Interfaces.Repository;
using Domain.Entity;
using Domain.Interfaces.Service;
using Service.Helper;

namespace Service.Service;

public class ServiceCadastroPaciente(IRepositoryPaciente repositorioPaciente, HelperTransacao helperTransacao) : IServiceCadastroPaciente
{
    public async Task<Paciente?> ResgatarPacientePorEmail(string email) => await repositorioPaciente.ResgatarPacientePorEmail(email);

    public async Task GravarPaciente(Paciente paciente)
    {
        using (var transacao = helperTransacao.CriaTransacao())
        {
            ArgumentNullException.ThrowIfNull(paciente);

            paciente.Validar();

            if (paciente.Id == null)
                await repositorioPaciente.RegistarNovoPaciente(paciente);
            else
                await repositorioPaciente.AlterarDadosPaciente(paciente);

            transacao.Gravar();
        }
    }

    public async Task ExcluirPaciente(int id)
    {
        using (var transacao = helperTransacao.CriaTransacao())
            await repositorioPaciente.ExcluirPaciente(id);
    }
}
