CREATE TABLE Paciente 
(
    Id SERIAL PRIMARY KEY,
    Nome VARCHAR(255) NOT NULL,
    CPF VARCHAR(11) NOT NULL UNIQUE,
    EMail VARCHAR(255) NOT NULL UNIQUE
);

CREATE TABLE Medico 
(
    Id SERIAL PRIMARY KEY,
    Nome VARCHAR(255) NOT NULL,
    CPF VARCHAR(11) NOT NULL UNIQUE,
    EMail VARCHAR(255) NOT NULL UNIQUE,
    CRM VARCHAR(25) NOT NULL UNIQUE,
    Especialidade VARCHAR(100) NOT NULL,
    ValorConsulta NUMERIC(10, 2) NOT NULL CHECK (ValorConsulta >= 0)
);

CREATE TABLE HorarioMedico 
(
    Id SERIAL PRIMARY KEY,
    IdMedico INT NOT NULL,
    DiaSemana INT NOT NULL,
    HoraInicial TIME NOT NULL,
    HoraFinal TIME NOT NULL,
    CONSTRAINT fk_medico
        FOREIGN KEY (IdMedico) 
        REFERENCES Medico (Id)
);

CREATE TYPE StatusConsulta AS ENUM ('Agendada', 'Realizada', 'Cancelada');

CREATE TABLE Consulta 
(
    Id SERIAL PRIMARY KEY,
    IdPaciente INT NOT NULL,
    IdMedico INT NOT NULL,
    DataHora TIMESTAMP NOT NULL,
    StatusConsulta StatusConsulta NOT NULL,
    JustificativaCancelamento VARCHAR(255),
    CONSTRAINT fk_paciente
        FOREIGN KEY (IdPaciente) 
        REFERENCES Paciente (Id),
    CONSTRAINT fk_medico
        FOREIGN KEY (IdMedico) 
        REFERENCES Medico (Id)
);


