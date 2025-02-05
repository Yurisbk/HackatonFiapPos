using Domain.Entity;

namespace Tests.Integration.Helper;

public class HelperGeracaoEntidades()
{
    static int counter;

    static public string GerarCpf()
    {
        int soma = 0, resto = 0;
        int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        Random rnd = new Random();
        string semente = rnd.Next(100000000, 999999999).ToString();

        for (int i = 0; i < 9; i++)
            soma += int.Parse(semente[i].ToString()) * multiplicador1[i];

        resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        semente = semente + resto;
        soma = 0;

        for (int i = 0; i < 10; i++)
            soma += int.Parse(semente[i].ToString()) * multiplicador2[i];

        resto = soma % 11;

        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        semente = semente + resto;
        return semente;
    }

    public static string GerarCrm()
    {
        Random rnd = new Random();
        int numero = rnd.Next(100000, 999999);
        string letra = ((char)rnd.Next('A', 'Z' + 1)).ToString();
        return $"{numero}{letra}";
    }

    static public Paciente CriaPacienteValido() => new Paciente() { Nome = $"Paciente {++counter}", CPF = GerarCpf(), EMail = $"paciente{counter}@teste.com" };

    static public Medico CriaMedicoValido()
    {
        string especialidade = EspecialidadesMedicas[new Random().Next(0, EspecialidadesMedicas.Length)];
        return new Medico() { Nome = $"Medico {++counter}", CPF = GerarCpf(), EMail = $"medico{counter}@teste.com", Especialidade = especialidade, CRM = GerarCrm() };
    }

    static public string[] EspecialidadesMedicas =
    [
        "Cardiologia",
        "Dermatologia",
        "Endocrinologia",
        "Gastroenterologia",
        "Neurologia",
        "Oftalmologia",
        "Ortopedia",
        "Pediatria",
        "Psiquiatria",
        "Urologia"
    ];
}
