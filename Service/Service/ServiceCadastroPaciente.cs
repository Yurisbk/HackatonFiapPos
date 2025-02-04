using Domain.Interfaces.Repository;
using Domain.Entity;
using Domain.Interfaces.Service;

namespace Service.Service;

public class ServiceCadastroPaciente(IRepositoryPaciente repositorioPaciente) : IServiceCadastroPaciente
{
    public async Task<Paciente?> ResgatarPacientePorEmail(string email) => await repositorioPaciente.ResgatarPacientePorEmail(email);

    public async Task GravarPaciente(Paciente paciente)
    {
        ArgumentNullException.ThrowIfNull(paciente);

        paciente.Validar();

        if (paciente.Id == null)
            await repositorioPaciente.RegistarNovoPaciente(paciente);
        else
            await repositorioPaciente.AlterarDadosPaciente(paciente);
    }

    public async Task ExcluirPaciente(int id) => await repositorioPaciente.ExcluirPaciente(id);
}
