namespace Domain.Enum;

public enum StatusConsulta 
{ 
    // Consulta foi criada pelo paciente e aguarda confirmação do médico
    Pendente, 
    // Consulta confirmada pelo médico
    Confirmada, 
    // Conculsta recusada pelo médico
    Recusada,
    // Consulta cancelada pelo paciente
    Cancelada
};
