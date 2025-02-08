using Domain.DTO;
using Domain.Entity;
using Domain.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;

namespace AgendamentoConsultasMedicas.Controllers;

[ApiController]
[Route("api/paciente")]
public class ControllerPaciente(IServiceCadastroPaciente serviceCadastroPaciente): ControllerBase
{
    [HttpPost]
    [Route("gravar")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiError))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiError))]
    public async Task<IActionResult> CadastrarPaciente([FromBody] Paciente paciente)
    {
        await serviceCadastroPaciente.GravarPaciente(paciente);

        return Created();
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

    [HttpGet]
    [Route("resgatarPorEmail")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Paciente))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiError))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiError))]
    public async Task<IActionResult> ResgatarPorEmail([FromQuery] string email)
    {
        return Ok(await serviceCadastroPaciente.ResgatarPacientePorEmail(email));
    }
}
