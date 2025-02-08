using Domain.Interfaces.Repository;
using Domain.Entity;
using Domain.Interfaces.Service;
using Service.Helper;
using Domain.Enum;

namespace Service.Service;

public class ServiceCadastroPaciente(
    IRepositoryPaciente repositorioPaciente, 
    IServiceConsulta serviceConsulta, 
    HelperTransacao helperTransacao) : IServiceCadastroPaciente
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
        {
            var consultasAtivas = await serviceConsulta.ListarConsultasAtivasPaciente(id);
            foreach (var consultaAtiva in consultasAtivas)
                await serviceConsulta.GravarStatusConsulta(consultaAtiva.Id!.Value, StatusConsulta.Cancelada, "Paciente desligado do sistema.");

            await repositorioPaciente.ExcluirPaciente(id);

            transacao.Gravar();
        }
    }
}
