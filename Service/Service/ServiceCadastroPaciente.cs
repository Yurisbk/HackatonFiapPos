using Domain.Interfaces.Repository;
using Domain.Entity;
using Domain.Interfaces.Service;

namespace Services;

public class ServiceCadastroPaciente(IRepositorioPaciente repositorioPaciente): IServiceCadastroPaciente
{
    public Paciente? ResgatarPacientePorEmail(string email) => repositorioPaciente.ResgatarPacientePorEmail(email);

    public void GravarPaciente(Paciente paciente)
    {
        ArgumentNullException.ThrowIfNull(paciente);

        paciente.Validar();

        if (paciente.Id == null)
            repositorioPaciente.RegistarNovoPaciente(paciente);
        else
            repositorioPaciente.AlterarDadosPaciente(paciente);
    }

    public void ExcluirPaciente(int id) => repositorioPaciente.ExcluirPaciente(id);
}
