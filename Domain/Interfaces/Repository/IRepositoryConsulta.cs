using Domain.Entity;

namespace Domain.Interfaces.Repository;

public interface IRepositoryConsulta
{
    Task<Consulta?> ResgatarConsultaPorId(int idConsulta);
    Task RegistrarConsulta(Consulta consulta);
    Task<Consulta[]> ListarConsultasPendentesConfirmacaoMedico(int idMedico);
    Task<Consulta[]> ListarConsultasAtivasPaciente(int idPaciente, DateTime? data = null);
    Task<Consulta[]> ListarConsultasAtivasMedico(int idMedico, DateTime? data = null);
    Task GravarStatusConsulta(Consulta consulta);
}