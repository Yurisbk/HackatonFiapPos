using Domain.DTO.Autenticacao;
using Domain.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AutenticadorController : ControllerBase
    {
        private readonly IServiceAuthenticacao _authentication;

        public AutenticadorController(IServiceAuthenticacao authentication)
        {
            _authentication = authentication;
        }
        [Authorize]
        [HttpGet("validar-token")]
        public async Task<IActionResult> Valida()
        {
            return Ok();
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] DTOCreateUsuario request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authentication.Register(request);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Paciente registrado com sucesso!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] DTOLoginUsuario request)
        {
            try
            {
                var token = await _authentication.Login(request);
                return Ok(new { access_token = token });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authentication.Logout(User);

            return Ok(new { message = "Logout realizado com sucesso!" });
        }
    }
}
