﻿using Domain.DTO.Autenticacao;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Domain.Interfaces.Service;
public interface IServiceAuthenticacao
{
    Task<IdentityResult> Register(DTOCreateUsuario request);
    Task<string?> Login(DTOLoginUsuario request);
    Task Logout(ClaimsPrincipal user);
}
