using Domain.DTO;
using Domain.Interfaces.Service;
using Microsoft.Extensions.DependencyInjection;
using Tests.Integration.Fixture;

namespace Tests.Integration;

public class TestsConsulta: TestsBaseDI
{
    public TestsConsulta(FixtureDI fixtureDI) : base(fixtureDI)
    {
    }

    [Fact]
    public void TestarMarcacaoConsulta()
    {
        var serviceCadastroPaciente = ServiceProvider.GetService<IServiceCadastroPaciente>()!;
        var serviceCadastroMedico = ServiceProvider.GetService<IServiceCadastroMedico>()!;
        var serviceHorarioMedico = ServiceProvider.GetService<IServiceHorarioMedico>()!;
        var serviceConsulta = ServiceProvider.GetService<IServiceConsulta>()!;

        serviceCadastroPaciente.GravarPaciente(new Paciente() { CPF = "123.456.789-09", Nome = "Paciente Teste", EMail = "teste@teste.com" });
        var paciente = serviceCadastroPaciente.ResgatarPacientePorEmail("teste@teste.com")!;

        serviceCadastroMedico.GravarMedico(new Medico() { CPF = "123.456.789-09", Nome = "Medico Teste", EMail = "teste@teste.com", CRM = "1234" });
        var medico = serviceCadastroMedico.ResgatarMedicoPorEmail("teste@teste.com")!;

        serviceHorarioMedico.RegistrarHorariosMedicoDiaSemana(medico.Id!.Value, DayOfWeek.Thursday, new Periodo(8, 12), new Periodo(14, 18));

        var horariosMedicos = serviceHorarioMedico.ListarHorariosMedicoDiaSemana(DayOfWeek.Thursday);

        var horariosLivres = serviceConsulta.ListarHorariosLivres(1);

        serviceConsulta.RegistrarConsulta(paciente.Id!.Value, medico.Id!.Value, horariosLivres[1].Horario);

        horariosLivres = serviceConsulta.ListarHorariosLivres(1);
    }
}