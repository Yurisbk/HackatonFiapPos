using Domain.DTO;
using Domain.Entity;
using Domain.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgendamentoConsultasMedicas.Controllers;

[ApiController]
[Route("api/paciente")]
public class ControllerPaciente(IServiceCadastroPaciente serviceCadastroPaciente): ControllerBase
{
    [HttpPost]
    [Route("gravar")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiError))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiError))]
    public async Task<IActionResult> GravarPaciente([FromBody] DTOPaciente paciente)
    {
        await serviceCadastroPaciente.GravarPaciente((Paciente)paciente!);

        return Accepted();
    }

    [HttpDelete]
    [Route("excluir")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiError))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiError))]
    public async Task<IActionResult> ExcluirPaciente([FromQuery] int id)
    {
        await serviceCadastroPaciente.ExcluirPaciente(id);

        return Accepted();
    }
    [Authorize]
    [HttpGet]
    [Route("resgatarPorEmail")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTOPaciente))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiError))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiError))]
    public async Task<IActionResult> ResgatarPorEmail([FromQuery] string email)
    {
        return Ok((DTOPaciente?)(await serviceCadastroPaciente.ResgatarPacientePorEmail(email)));
    }
}
