using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.Interfaces.Service;
using Domain.DTO;
using Microsoft.AspNetCore.Identity.Data;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using Domain.DTO.Autenticacao;

namespace AgendamentoConsultasMedicas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacienteController : ControllerBase
    {
        private readonly IServiceCadastroPaciente _serviceCadastroPaciente;
        public PacienteController(IServiceCadastroPaciente serviceCadastroPaciente)
        {
            _serviceCadastroPaciente = serviceCadastroPaciente;
        }

        [Authorize]
        [HttpGet("ValidaToken")]
        public IActionResult ValidaToken()
        {
            return Ok("Token válidado com sucesso!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] DTOLoginPaciente loginRequest)
        {
            try
            {
                var authResponse = await _serviceCadastroPaciente.LoginPaciente(loginRequest);
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
        public async Task<IActionResult> Cadastrar([FromBody] DTOCreatePaciente createPaciente)
        {
            try
            {
                DTOCreateUsuarioResponse? authResponse = await _serviceCadastroPaciente.CriarPaciente(createPaciente);

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
            var result = _serviceCadastroPaciente.ResgatarPacientePorEmail(email);

            return Ok(result);
        }
    }
}
