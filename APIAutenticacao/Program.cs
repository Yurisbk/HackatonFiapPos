using ContatoAPI.Extension;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddInjecoesDependencias();
builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddIdentityConfiguration(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDocumentacaoSwagger();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await IdentityRoles.SeedRoles(roleManager);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseMiddleware<TokenListaNegraMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

#region .::Prometheus::.
app.UseMetricServer(); //endpoint /metrics
app.UseHttpMetrics();
#endregion

app.MapControllers();

app.Run();
