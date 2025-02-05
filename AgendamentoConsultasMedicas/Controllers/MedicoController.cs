using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.Interfaces.Service;
using Domain.DTO;
using Service.Service;

namespace AgendamentoConsultasMedicas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicoController : ControllerBase
    {
        private readonly IServiceCadastroMedico _serviceCadastroMedico;
        public MedicoController(IServiceCadastroMedico serviceCadastroMedico)
        {
            _serviceCadastroMedico = serviceCadastroMedico;
        }

        [Authorize]
        [HttpGet("ValidaToken")]
        public IActionResult ValidaToken()
        {
            return Ok("Token válidado com sucesso!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] DTOLoginMedico loginRequest)
        {
            try
            {
                var authResponse = await _serviceCadastroMedico.LoginMedico(loginRequest);
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



        [HttpPost("cadastrar")]
        public async Task<IActionResult> Cadastrar([FromBody] DTOCreateMedico createMedico)
        {
            try
            {
                DTOCreateUsuarioResponse? authResponse = await _serviceCadastroMedico.CriarMedico(createMedico);

                return Ok(authResponse);
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }

        }

        [HttpGet("listar")]
        public async Task<IActionResult> Listar(string email)
        {
            var result = _serviceCadastroMedico.ResgatarMedicoPorEmail(email);

            return Ok(result);
        }
    }
}
