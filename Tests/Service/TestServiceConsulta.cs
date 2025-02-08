using Domain.Entity;
using Domain.Enum;
using Tests.Helper;

namespace Tests.Service;

public class TestServiceConsulta : TestServiceBase
{
    [Fact]
    public async Task TestaRegistroConsultas()
    {
        // Cria e grava pacientes validos

        var paciente1 = HelperGeracaoEntidades.CriaPacienteValido();
        await ServiceCadastroPaciente.GravarPaciente(paciente1);

        var paciente2 = HelperGeracaoEntidades.CriaPacienteValido();
        await ServiceCadastroPaciente.GravarPaciente(paciente2);

        // Cria e grava medicos validos em 2 especialidades 

        const string especialidade1 = "especialidade1";
        const string especialidade2 = "especialidade2";

        var medico1 = HelperGeracaoEntidades.CriaMedicoValido();
        medico1.Especialidade = especialidade1;
        await ServiceCadastroMedico.GravarMedico(medico1);
        await ServiceHorarioMedico.RegistrarHorariosMedicoDiaSemana(medico1.Id!.Value, DateTime.Today.DayOfWeek, [new Periodo(8, 17)]);

        var medico2 = HelperGeracaoEntidades.CriaMedicoValido();
        medico1.Especialidade = especialidade2;
        await ServiceCadastroMedico.GravarMedico(medico2);
        await ServiceHorarioMedico.RegistrarHorariosMedicoDiaSemana(medico2.Id!.Value, DateTime.Today.DayOfWeek, [new Periodo(8, 17)]);

        var horariosLivres = await ServiceConsulta.ListarAgendaMedico(medico1.Id!.Value);
        Assert.Equal(10, horariosLivres.Length);

        // Testa agendamento de consultas

        // Não pode permitir agendar fora do horario de atendimento do medico
        await Assert.ThrowsAnyAsync<Exception>(async () => await ServiceConsulta.RegistrarConsulta(medico1.Id!.Value, paciente1.Id!.Value, DateTime.Today.AddHours(7)));

        await ServiceConsulta.RegistrarConsulta(medico1.Id!.Value, paciente1.Id!.Value, DateTime.Today.AddHours(8));

        // Não pode permitir agendar 2 consultas no mesmo horario, nem para omedico, nem para o paciente
        await Assert.ThrowsAnyAsync<Exception>(async () => await ServiceConsulta.RegistrarConsulta(medico1.Id!.Value, paciente2.Id!.Value, DateTime.Today.AddHours(8)));
        await Assert.ThrowsAnyAsync<Exception>(async () => await ServiceConsulta.RegistrarConsulta(medico2.Id!.Value, paciente1.Id!.Value, DateTime.Today.AddHours(8)));

        // Deve regirar horario ocupado na agenda livre do medico
        horariosLivres = await ServiceConsulta.ListarAgendaMedico(medico1.Id!.Value);
        Assert.Equal(9, horariosLivres.Length);

        // Marca outra consulta
        await ServiceConsulta.RegistrarConsulta(medico1.Id!.Value, paciente2.Id!.Value, DateTime.Today.AddHours(14));

        // Deve regirar horario ocupado na agenda livre do medico
        horariosLivres = await ServiceConsulta.ListarAgendaMedico(medico1.Id!.Value);
        Assert.Equal(8, horariosLivres.Length);

        // Testa listagem de consultas pendentes de confirmação
        var consultas = await ServiceConsulta.ListarConsultasPendentesConfirmacaoMedico(medico1.Id!.Value);
        Assert.Equal(2, consultas.Length);

        // Testa confirmação de consulta
        await ServiceConsulta.GravarStatusConsulta(consultas[0].Id!.Value, StatusConsulta.Confirmada);
        await ServiceConsulta.GravarStatusConsulta(consultas[1].Id!.Value, StatusConsulta.Recusada);

        // Testa cancelamento de consulta
        await ServiceConsulta.GravarStatusConsulta(consultas[0].Id!.Value, StatusConsulta.Cancelada, "Teste cancelamento");

        // Deve regirar horario ocupado na agenda livre do medico
        horariosLivres = await ServiceConsulta.ListarAgendaMedico(medico1.Id!.Value);
        Assert.Equal(10, horariosLivres.Length);
    }
}
