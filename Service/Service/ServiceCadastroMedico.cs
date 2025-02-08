using Domain.Entity;
using Domain.Enum;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Service;

namespace Service.Service;

public class ServiceCadastroMedico(
    IRepositoryMedico repositorioMedico, 
    IServiceConsulta serviceConsulta, 
    ITransacaoFactory transacaoFactory): IServiceCadastroMedico
{
    public async Task<Medico?> ResgatarMedicoPorId(int id)
        => await repositorioMedico.ResgatarMedicoPorId(id);

    public async Task<Medico?> ResgatarMedicoPorCRM(string crm) 
        => await repositorioMedico.ResgatarMedicoPorCRM(crm);

    public async Task GravarMedico(Medico medico)
    {
        using (var transacao = transacaoFactory.CriaTransacao())
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
        using (var transacao = transacaoFactory.CriaTransacao())
        {
            // Lista e cancela consultas do medico com justificativa de exclusão
            var consultasAtivas = await serviceConsulta.ListarConsultasAtivasMedico(id, null);
            foreach (var consultaAtiva in consultasAtivas)
                await serviceConsulta.GravarStatusConsulta(consultaAtiva.Id!.Value, StatusConsulta.Cancelada, "Médico desligado do sistema.");

            transacao.Gravar();
        }
    }

    public async Task<string[]> ListarEspecialidadeMedicas() 
        => await repositorioMedico.ListarEspecialidadesMedicas();

    public async Task<Medico[]> ListarMedicosAtivosNaEspecialidade(string especialidade) 
        => await repositorioMedico.ListarMedicosAtivosNaEspecialidade(especialidade);
}
