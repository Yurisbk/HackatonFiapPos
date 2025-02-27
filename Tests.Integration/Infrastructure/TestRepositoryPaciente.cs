﻿using Domain.Interfaces.Repository;
using Domain.Interfaces.Service;
using Microsoft.Extensions.DependencyInjection;
using Tests.Helper;
using Tests.Integration.Fixture;

namespace Tests.Integration.Infrastructure;

public class TestRepositoryPaciente(WebAppFixture webAppFixture) : TestBaseWebApp(webAppFixture)
{
    IRepositoryPaciente repositoryPaciente => ServiceProvider.GetService<IRepositoryPaciente>()!;
    ITransacaoFactory transacaoFactory => ServiceProvider.GetService<ITransacaoFactory>()!;

    [Fact]
    public async Task TestCadastroPaciente()
    {
        using (var transacao = transacaoFactory.CriaTransacao())
        {
            // Testa registro

            var paciente = HelperGeracaoEntidades.CriaPacienteValido();
            await repositoryPaciente.RegistarNovoPaciente(paciente);

            // Testa Uks

            var outroPaciente = HelperGeracaoEntidades.CriaPacienteValido();
            outroPaciente.CPF = paciente.CPF;
            await Assert.ThrowsAsync<InvalidOperationException>(() => repositoryPaciente.RegistarNovoPaciente(paciente));

            outroPaciente = HelperGeracaoEntidades.CriaPacienteValido();
            outroPaciente.EMail = paciente.EMail;
            await Assert.ThrowsAsync<InvalidOperationException>(() => repositoryPaciente.RegistarNovoPaciente(paciente));

            // Testa resgates

            var pacienteCadastro = await repositoryPaciente.ResgatarPacientePorId(paciente.Id!.Value);
            Assert.Equal(paciente.CPF, pacienteCadastro?.CPF);

            pacienteCadastro = await repositoryPaciente.ResgatarPacientePorEmail(paciente.EMail!);
            Assert.Equal(paciente.EMail, pacienteCadastro?.EMail);

            // Testa alteração

            paciente.Nome = "Novo Nome";
            await repositoryPaciente.AlterarDadosPaciente(paciente);

            pacienteCadastro = await repositoryPaciente.ResgatarPacientePorId(paciente.Id!.Value);
            Assert.Equal(paciente.Nome, pacienteCadastro?.Nome);

            outroPaciente = HelperGeracaoEntidades.CriaPacienteValido();
            await repositoryPaciente.RegistarNovoPaciente(outroPaciente);

            // Testa UK em alteração

            outroPaciente.CPF = paciente.CPF;
            await Assert.ThrowsAsync<InvalidOperationException>(() => repositoryPaciente.AlterarDadosPaciente(outroPaciente));

            // Testa exclusão

            await repositoryPaciente.ExcluirPaciente(paciente.Id!.Value);
            pacienteCadastro = await repositoryPaciente.ResgatarPacientePorId(paciente.Id!.Value);
            Assert.Null(pacienteCadastro);
        }
    }
}
