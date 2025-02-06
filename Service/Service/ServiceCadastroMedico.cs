using Domain.Entity;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Service;
using Service.Helper;

namespace Service.Service;

public class ServiceCadastroMedico(IRepositoryMedico repositorioMedico, HelperTransacao helperTransacao): IServiceCadastroMedico
{
    public async Task<Medico?> ResgatarMedicoPorEmail(string email) => await repositorioMedico.ResgatarMedicoPorEmail(email);

    public async Task GravarMedico(Medico medico)
    {
        using (var transacao = helperTransacao.CriaTransacao())
        {
            medico.Validar();

            if (medico.Id == null)
                await repositorioMedico.RegistarNovoMedico(medico);
            else
                await repositorioMedico.AlterarDadosMedico(medico);

            transacao.Gravar();
        }
    }

    public async Task ExcluirMedico(int id)
    {
        using (var transacao = helperTransacao.CriaTransacao())
            await repositorioMedico.ExcluirMedico(id);
    }
}
