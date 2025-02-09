using Domain.Entity;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Service;
using Microsoft.Extensions.DependencyInjection;
using Tests.Helper;
using Tests.Integration.Fixture;

namespace Tests.Integration.Infrastructure;

public class TestRepositoryConsulta(WebAppFixture webAppFixture) : TestBaseWebApp(webAppFixture)
{
    IRepositoryConsulta repositoryConsulta => ServiceProvider.GetService<IRepositoryConsulta>()!;
    IRepositoryMedico repositoryMedico => ServiceProvider.GetService<IRepositoryMedico>()!;
    IRepositoryPaciente repositoryPaciente => ServiceProvider.GetService<IRepositoryPaciente>()!;
    ITransacaoFactory transacaoFactory => ServiceProvider.GetService<ITransacaoFactory>()!;

    [Fact]
    public async Task TestCadastroConsultas()
    {
        using (var transacao = transacaoFactory.CriaTransacao())
        {
            // Registra medico e paciente valido
            Medico medico = HelperGeracaoEntidades.CriaMedicoValido();
            await repositoryMedico.RegistarNovoMedico(medico);

            Paciente paciente = HelperGeracaoEntidades.CriaPacienteValido();
            await repositoryPaciente.RegistarNovoPaciente(paciente);

            // Registra consulta e assegura criação

            Consulta consulta = new Consulta() { IdMedico = medico.Id!.Value, IdPaciente = paciente.Id!.Value, DataHora = DateTime.Now };    
            await repositoryConsulta.RegistrarConsulta(consulta);
            Assert.NotNull(consulta.Id);

            // Testa regstate por id

            Consulta? consultaResgatada = await repositoryConsulta.ResgatarConsultaPorId(consulta.Id.Value);
            Assert.NotNull(consultaResgatada);

            // Assegura listagem de consultas corretas

            var consultas = await repositoryConsulta.ListarConsultasPendentesConfirmacaoMedico(medico.Id!.Value);
            Assert.Single(consultas);

            consultas = await repositoryConsulta.ListarConsultasAtivasMedico(medico.Id!.Value, DateTime.Today);
            Assert.Single(consultas);

            consultas = await repositoryConsulta.ListarConsultasAtivasPaciente(paciente.Id!.Value, DateTime.Today);
            Assert.Single(consultas);

            // Testa gravação de status da consulta

            consulta.StatusConsulta = Domain.Enum.StatusConsulta.Confirmada;
            await repositoryConsulta.GravarStatusConsulta(consulta);

            // Assegura troca de status

            consultas = await repositoryConsulta.ListarConsultasAtivasPaciente(paciente.Id!.Value, DateTime.Today);
            Assert.Single(consultas);
        }
    }
}
