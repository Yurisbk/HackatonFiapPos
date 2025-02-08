create table medico(
    id serial primary key,
    fullname varchar,
    cpf varchar,
    crm varchar,
    email varchar,
    passcode varchar
);

create table paciente(
    id serial primary key,
    fullname varchar,
    cpf varchar,
    email varchar,
    passcode varchar
);

create table consultas(
    id serial primary key,
    idmedico varchar,
    horalivre varchar,
    horariolivre varchar
);
