using Domain.DTO;
using Domain.Entity;
using Domain.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;
using Service.Service;

namespace AgendamentoConsultasMedicas.Controllers;

[ApiController]
[Route("api/medico")]
public class ControllerMedico(IServiceCadastroMedico serviceCadastroMedico, IServiceHorarioMedico serviceHorarioMedico) : ControllerBase
{

    [HttpPost]
    [Route("cadastrar")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiError))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiError))]
    public async Task<IActionResult> GravarMedico([FromBody] DTOCreateMedico paciente)
    {
        DTOCreateUsuarioResponse? authResponse = await serviceCadastroMedico.CriarMedico(paciente);

        return Accepted(authResponse.Auth_Id);
    }

    [HttpPost]
    [Route("login")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiError))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiError))]
    public async Task<IActionResult> Login([FromBody] DTOLoginMedico loginRequest)
    {
        try
        {
            var authResponse = await serviceCadastroMedico.LoginMedico(loginRequest);
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
    public async Task<IActionResult> ExcluirMedico([FromQuery] int id)
    {
        await serviceCadastroMedico.ExcluirMedico(id);

        return Accepted();
    }

    [HttpGet]
    [Route("resgatarPorCRM")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTOMedico))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiError))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiError))]
    public async Task<IActionResult> ResgatarPorEmail([FromQuery] string crm)
    {
        return Ok((DTOMedico?)await serviceCadastroMedico.ResgatarMedicoPorCRM(crm));
    }

    [HttpGet]
    [Route("listarEspecialidades")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string[]))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiError))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiError))]
    public async Task<IActionResult> ListarEspecialidades()
    {
        return Ok(await serviceCadastroMedico.ListarEspecialidadeMedicas());
    }

    [HttpGet]
    [Route("listarMedicosEspecialidade")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTOMedico[]))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiError))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiError))]
    public async Task<IActionResult> ListarMedicosEspecialidade([FromQuery] string especialidade)
    {
        var medicos = await serviceCadastroMedico.ListarMedicosAtivosNaEspecialidade(especialidade);

        return Ok(medicos.Select(m => (DTOMedico?)m).ToArray());
    }

    [HttpPost]
    [Route("registrarHorarioMedicoDiaSemana")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiError))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiError))]
    public async Task<IActionResult> RegistrarHorariosMedico([FromBody] DTOHorarioMedicoDiaSemana horarioMedicoDiaSemana)
    {
        await serviceHorarioMedico.RegistrarHorariosMedicoDiaSemana(horarioMedicoDiaSemana.IdMedico, horarioMedicoDiaSemana.DiaSemana, horarioMedicoDiaSemana.Horarios!);

        return Accepted();
    }
}
