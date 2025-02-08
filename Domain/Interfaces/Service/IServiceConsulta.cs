using Domain.DTO;
using Domain.Entity;
using Domain.Enum;

namespace Domain.Interfaces.Service;

public interface IServiceConsulta
{
    Task<DTOHorariosLivre[]> ListarAgendaMedico(int idMedico, int dias = 7);
    Task<Consulta[]> ListarConsultasPendentesConfirmacaoMedico(int idMedico);
    Task<Consulta[]> ListarConsultasAtivasPaciente(int idPaciente, DateTime? data = null);
    Task<Consulta[]> ListarConsultasAtivasMedico(int idMedico, DateTime? data = null);
    Task GravarStatusConsulta(int idConsulta, StatusConsulta statusConsulta, string justificativaCancelamento);
}