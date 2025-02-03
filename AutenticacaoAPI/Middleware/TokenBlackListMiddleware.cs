//using System.IdentityModel.Tokens.Jwt;
//using Microsoft.AspNetCore.Identity;
//using AuthServer.Models;

//public class TokenBlacklistMiddleware
//{
//    private readonly RequestDelegate _next;
//    private readonly IServiceScopeFactory _scopeFactory;

//    public TokenBlacklistMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
//    {
//        _next = next;
//        _scopeFactory = scopeFactory;
//    }

//    public async Task Invoke(HttpContext context)
//    {
//        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
//        if (string.IsNullOrEmpty(token))
//        {
//            await _next(context);
//            return;
//        }

//        var handler = new JwtSecurityTokenHandler();
//        var jwtToken = handler.ReadJwtToken(token);
//        var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value
//                     ?? jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;

//        if (string.IsNullOrEmpty(userId))
//        {
//            context.Response.StatusCode = 401;
//            await context.Response.WriteAsync("Token inválido: usuário não encontrado.");
//            return;
//        }

//        using var scope = _scopeFactory.CreateScope();
//        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
//        var user = await userManager.FindByIdAsync(userId);

//        if (user == null)
//        {
//            context.Response.StatusCode = 401;
//            await context.Response.WriteAsync("Usuário não encontrado.");
//            return;
//        }

//        var storedToken = await userManager.GetAuthenticationTokenAsync(user, "JWT", "AccessToken");
//        if (string.IsNullOrEmpty(storedToken) || storedToken != token)
//        {
//            context.Response.StatusCode = 401;
//            await context.Response.WriteAsync("Token revogado.");
//            return;
//        }

//        await _next(context);
//    }
//}
