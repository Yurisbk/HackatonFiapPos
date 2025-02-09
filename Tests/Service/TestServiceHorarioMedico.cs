using Domain.Entity;
using Service.Service;
using Tests.Helper;

namespace Tests.Service;

public class TestServiceHorarioMedico: TestServiceBase
{
    [Fact]
    public async Task TestCadastroHorarioMedico()
    {
        using (var transacao = TransacaoFactory.CriaTransacao())
        {
            Medico medico = HelperGeracaoEntidades.CriaMedicoValido()!;
            await ServiceCadastroMedico.GravarMedico(medico);

            // Testa horarios invalidos

            await Assert.ThrowsAnyAsync<Exception>(() => ServiceHorarioMedico.RegistrarHorariosMedicoDiaSemana(-1, DayOfWeek.Monday, []));
            await Assert.ThrowsAnyAsync<Exception>(() => ServiceHorarioMedico.RegistrarHorariosMedicoDiaSemana(medico.Id!.Value, DayOfWeek.Monday, new Periodo(8, 12), new Periodo(11, 14)));

            // Testa e assegura gravaão de horarios validos

            await ServiceHorarioMedico.RegistrarHorariosMedicoDiaSemana(medico.Id!.Value, DayOfWeek.Monday, new Periodo(8, 12), new Periodo(13, 14));

            var horarios = await ServiceHorarioMedico.ResgatarHorariosMedicoDiaSemana(medico.Id!.Value, DayOfWeek.Monday);

            Assert.True(horarios.Length == 2);
        }
    }
}
