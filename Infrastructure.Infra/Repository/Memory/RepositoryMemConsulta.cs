using Domain.Entity;
using Domain.Enum;
using Domain.Interfaces.Repository;

namespace Infrastructure.Repository.Memory;

public class RepositoryMemConsulta : IRepositoryConsulta
{
    public async Task RegistrarConsulta(Consulta consulta)
    {
        await MemDB.DBLock.WaitAsync();
        try
        {
            ArgumentNullException.ThrowIfNull(consulta);

            MemDB.Medicos.CheckFK(consulta.IdMedico, "Medico não encontrado.");
            MemDB.Pacientes.CheckFK(consulta.IdPaciente, "Paciente não encontrado.");

            MemDB.Consultas.CheckUK(c => c.IdMedico == consulta.IdMedico && c.DataHora == consulta.DataHora  && (c.StatusConsulta == StatusConsulta.Pendente || c.StatusConsulta == StatusConsulta.Confirmada), "Medico ja tem consulta marcada neste horário.");
            MemDB.Consultas.CheckUK(c => c.IdPaciente == consulta.IdPaciente && c.DataHora == consulta.DataHora && (c.StatusConsulta == StatusConsulta.Pendente || c.StatusConsulta == StatusConsulta.Confirmada), "Paciente ja tem outra consulta marcada neste horário.");

            MemDB.Consultas.Insert(consulta);
        }
        finally
        {
            MemDB.DBLock.Release();
        }
    }

    public async Task<Consulta?> ResgatarConsultaPorId(int idConsulta)
    {
        await MemDB.DBLock.WaitAsync();
        try
        {
            return MemDB.Consultas.FirstOrDefault(c => c.Id == idConsulta);
        }
        finally
        {
            MemDB.DBLock.Release();
        }
    }

    public async Task<Consulta[]> ListarConsultasPendentesConfirmacaoMedico(int idMedico)
    {
        await MemDB.DBLock.WaitAsync();
        try
        {
            return MemDB.Consultas.Where(c => c.StatusConsulta == StatusConsulta.Pendente && c.IdMedico == idMedico).ToArray();
        }
        finally
        {
            MemDB.DBLock.Release();
        }
    }

    public async Task<Consulta[]> ListarConsultasAtivasPaciente(int idPaciente, DateTime? data = null)
    {
        await MemDB.DBLock.WaitAsync();
        try
        {
            return MemDB.Consultas.Where(c => (c.StatusConsulta == StatusConsulta.Pendente || c.StatusConsulta == StatusConsulta.Confirmada) && c.DataHora.Date == (data?.Date ?? c.DataHora.Date) && c.IdPaciente == idPaciente).ToArray();
        }
        finally
        {
            MemDB.DBLock.Release();
        }
    }

    public async Task<Consulta[]> ListarConsultasAtivasMedico(int idMedico, DateTime? data = null)
    {
        await MemDB.DBLock.WaitAsync();
        try
        {
            MemDB.Medicos.CheckFK(idMedico, "Medico não encontrado.");

            return MemDB.Consultas.Where(c => c.IdMedico == idMedico && c.DataHora.Date == (data?.Date ?? c.DataHora.Date) && (c.StatusConsulta == StatusConsulta.Pendente || c.StatusConsulta == StatusConsulta.Confirmada)).ToArray();
        }
        finally
        {
            MemDB.DBLock.Release();
        }
    }

    public async Task GravarStatusConsulta(Consulta consulta)
    {
        await MemDB.DBLock.WaitAsync();
        try
        {
            MemDB.Consultas.Update(consulta);
        }
        finally
        {
            MemDB.DBLock.Release();
        }
    }
}
