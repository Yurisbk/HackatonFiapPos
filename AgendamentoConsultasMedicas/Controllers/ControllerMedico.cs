using Domain.DTO;
using Domain.Entity;
using Domain.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;

namespace AgendamentoConsultasMedicas.Controllers;

[ApiController]
[Route("api/medico")]
public class ControllerMedico(IServiceCadastroMedico serviceCadastroMedico, IServiceHorarioMedico serviceHorarioMedico) : ControllerBase
{
    [HttpPost]
    [Route("gravar")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiError))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiError))]
    public async Task<IActionResult> GravarMedico([FromBody] DTOMedico medico)
    {
        await serviceCadastroMedico.GravarMedico((Medico)medico!);

        return Accepted();
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
