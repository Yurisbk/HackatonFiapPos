using Domain.Entity;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Service;

namespace Service.Service;

public class ServiceCadastroMedico(IRepositoryMedico repositorioMedico): IServiceCadastroMedico
{
    public async Task<Medico?> ResgatarMedicoPorEmail(string email) => await repositorioMedico.ResgatarMedicoPorEmail(email);

    public async Task GravarMedico(Medico medico)
    {
        ArgumentNullException.ThrowIfNull(medico);

        medico.Validar();

        if (medico.Id == null)
            await repositorioMedico.RegistarNovoMedico(medico);
        else
            await repositorioMedico.AlterarDadosMedico(medico);
    }

    public async Task ExcluirMedico(int id) => await repositorioMedico.ExcluirMedico(id);
}
