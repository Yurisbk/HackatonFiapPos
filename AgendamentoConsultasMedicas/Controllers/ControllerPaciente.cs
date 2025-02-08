using Domain.DTO;
using Domain.Entity;
using Domain.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Service;

namespace AgendamentoConsultasMedicas.Controllers;

[ApiController]
[Route("api/paciente")]
public class ControllerPaciente(IServiceCadastroPaciente serviceCadastroPaciente) : ControllerBase
{
    [HttpPost]
    [Route("cadastrar")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiError))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiError))]
    public async Task<IActionResult> GravarPaciente([FromBody] DTOCreatePaciente paciente)
    {
        DTOCreateUsuarioResponse? authResponse = await serviceCadastroPaciente.CriarPaciente(paciente);

        return Accepted(authResponse.Auth_Id);
    }
    [HttpPost]
    [Route("login")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiError))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiError))]
    public async Task<IActionResult> Login([FromBody] DTOLoginPaciente loginRequest)
    {
        try
        {
            var authResponse = await serviceCadastroPaciente.LoginPaciente(loginRequest);
            return Ok(authResponse);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
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
