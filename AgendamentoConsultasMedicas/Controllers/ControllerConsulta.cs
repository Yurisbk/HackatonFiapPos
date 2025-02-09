using Domain.DTO;
using Domain.Entity;
using Domain.Enum;
using Domain.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;

namespace AgendamentoConsultasMedicas.Controllers;

public class ControllerConsulta(IServiceConsulta serviceConsulta) : ControllerBase
{
    [HttpGet]
    [Route("listarAgendaMedico")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTOHorariosLivre[]))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiError))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiError))]
    public async Task<IActionResult> ListarAgendaMedico([FromQuery] int idMedico, [FromQuery] int dias = 7)
    {
        return Ok(await serviceConsulta.ListarAgendaMedico(idMedico, dias));
    }

    [HttpPost]
    [Route("registraConsulta")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiError))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiError))]
    public async Task<IActionResult> RegistrarConsulta([FromQuery] int idMedico, [FromQuery] int idPaciente, [FromQuery] DateTime data)
    {
        await serviceConsulta.RegistrarConsulta(idMedico, idPaciente, data);

        return Accepted();
    }

    [HttpPatch]
    [Route("gravarStatusConsulta")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiError))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiError))]
    public async Task<IActionResult> GravarStatusConsulta([FromQuery] int idConsulta, [FromQuery] StatusConsulta statusConsulta, [FromQuery] string justificativa)
    {
        await serviceConsulta.GravarStatusConsulta(idConsulta, statusConsulta, justificativa);

        return Accepted();
    }

    [HttpGet]
    [Route("listarConsultasPendentesConfirmacaoMedico")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Consulta[]))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiError))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiError))]
    public async Task<IActionResult> ListarConsultasPendentesConfirmacaoMedico([FromQuery] int idMedico)
    {
        return Ok(await serviceConsulta.ListarConsultasPendentesConfirmacaoMedico(idMedico));
    }

    [HttpGet]
    [Route("listarConsultasAtivasPaciente")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Consulta[]))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiError))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiError))]
    public async Task<IActionResult> ListarConsultasAtivasPaciente([FromQuery] int idPaciente, [FromQuery] DateTime? data = null)
    {
        return Ok(await serviceConsulta.ListarConsultasAtivasPaciente(idPaciente, data));
    }

    [HttpGet]
    [Route("listarConsultasAtivasMedico")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Consulta[]))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiError))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiError))]
    public async Task<IActionResult> ListarConsultasAtivasMedico([FromQuery] int idMedico, [FromQuery] DateTime? data = null)
    {
        return Ok(await serviceConsulta.ListarConsultasAtivasMedico(idMedico, data));
    }
}