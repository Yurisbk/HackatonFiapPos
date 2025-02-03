using Domain.DTO;
using Domain.DTO.Autenticacao;
using Domain.Interfaces.Service;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Service.Service;


public class ServiceAutenticacao : IServiceAuthenticacao
{
    private readonly UserManager<Usuario> _userManager;
    private readonly SignInManager<Usuario> _signInManager;
    private readonly IJwtTokenService _jwtTokenService;

    public ServiceAutenticacao(
        UserManager<Usuario> userManager,
        SignInManager<Usuario> signInManager,
        IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<IdentityResult> Register(DTOCreateUsuario request)
    {
        var user = new Usuario
        {
            UserName = request.Email,
            Email = request.Email
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        var roles = Enum.GetValues<UserRoles>().Select(e => e.ToString()).ToList();
        await _userManager.AddToRolesAsync(user, roles);


        return result;
    }
    private async Task<bool> SaveTokenAsync(Usuario user, string token)
    {
        try
        {
            var existingToken = await _userManager.GetAuthenticationTokenAsync(user, "JWT", "AccessToken");

            if (!string.IsNullOrEmpty(existingToken))
            {
                await _userManager.RemoveAuthenticationTokenAsync(user, "JWT", "AccessToken");
            }

            await _userManager.SetAuthenticationTokenAsync(user, "JWT", "AccessToken", token);
            return true;
        }
        catch
        {
            return false;
        }
    }
    public async Task<string?> Login(DTOLoginUsuario request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null) throw new Exception("Usuário ou senha inválidos!");

        var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, lockoutOnFailure: true);
        if (result.IsLockedOut) throw new Exception("Usuário bloqueado! Tente novamente mais tarde.");
        if (!result.Succeeded) throw new Exception("Usuário ou senha inválidos!");

        var roles = await _userManager.GetRolesAsync(user);

        var token = _jwtTokenService.GenerateToken(user.Id, user.UserName, roles);

        var tokenSaved = await SaveTokenAsync(user, token);

        if (!tokenSaved) throw new Exception("Erro ao salvar o token.");

        return token;
    }

    public async Task Logout(ClaimsPrincipal userLogado)
    {
        var user = await _userManager.GetUserAsync(userLogado);

        if (user == null) throw new Exception("Ocorreu um problema para deslogar usuário não encontrado.");

        await _userManager.RemoveAuthenticationTokenAsync(user, "JWT", "AccessToken");
    }
}

