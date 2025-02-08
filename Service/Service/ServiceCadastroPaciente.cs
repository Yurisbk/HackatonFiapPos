using Domain.Entity;
using Domain.Enum;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Service;

namespace Service.Service;

public class ServiceCadastroPaciente(
    IRepositoryPaciente repositorioPaciente, 
    IServiceConsulta serviceConsulta, 
    ITransacaoFactory transacaoFactory) : IServiceCadastroPaciente
{
    public async Task<Paciente?> ResgatarPacientePorEmail(string email) => await repositorioPaciente.ResgatarPacientePorEmail(email);

    public async Task GravarPaciente(Paciente paciente)
    {
        using (var transacao = transacaoFactory.CriaTransacao())
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
        using (var transacao = transacaoFactory.CriaTransacao())
        {
            var consultasAtivas = await serviceConsulta.ListarConsultasAtivasPaciente(id);
            foreach (var consultaAtiva in consultasAtivas)
                await serviceConsulta.GravarStatusConsulta(consultaAtiva.Id!.Value, StatusConsulta.Cancelada, "Paciente desligado do sistema.");

            await repositorioPaciente.ExcluirPaciente(id);

            transacao.Gravar();
        }
    }
}
