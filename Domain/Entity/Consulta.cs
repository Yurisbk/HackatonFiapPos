﻿using Domain.Enum;

namespace Domain.Entity;

public class Consulta: Entidade
{
    public int IdPaciente;
    public int IdMedico;
    public DateTime DataHora;
    public AceiteMedico AceiteMedico;
}
