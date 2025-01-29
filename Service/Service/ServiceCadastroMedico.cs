using Domain.Entity;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Service;

namespace Service.Service;

public class ServiceCadastroMedico(IRepositorioMedico repositorioMedico): IServiceCadastroMedico
{
    public Medico? ResgatarMedicoPorEmail(string email) => repositorioMedico.ResgatarMedicoPorEmail(email);

    public void GravarMedico(Medico medico)
    {
        ArgumentNullException.ThrowIfNull(medico);

        medico.Validar();

        if (medico.Id == null)
            repositorioMedico.RegistarNovoMedico(medico);
        else
            repositorioMedico.AlterarDadosMedico(medico);
    }

    public void ExcluirMedico(int id) => repositorioMedico.ExcluirMedico(id);
}
