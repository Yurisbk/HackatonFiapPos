using Domain.Entity;

namespace Domain.Interfaces.Repository;

public interface IRepositoryConsulta
{
    Task<Consulta?> ResgatarConsultaPorId(int idConsulta);
    Task RegistrarConsulta(Consulta consulta);
    Task<Consulta[]> ListarConsultasPendentesConfirmacaoMedico(int idMedico);
    Task<Consulta[]> ListarConsultasAtivasPaciente(int idPaciente);
    Task<Consulta[]> ListarConsultasAtivasMedico(int idMedico, DateTime? data);
    Task GravarStatusConsulta(Consulta consulta);
}