**Health&Med** - Agendamento de Consultas é um sistema que permite o agendamento de consultas médicas, enviando notificações por e-mail para médicos e pacientes.

A arquitetura é baseada em uma API RESTful para gestão de consultas e um Consumer MassTransit que processa eventos assíncronos utilizando RabbitMQ.

Este projeto utiliza as seguintes tecnologias:

- .NET 8 - Framework principal da aplicação
- ASP.NET Core Identity - Gerenciamento de usuários e autenticação
- Entity Framework Core - ORM para acesso ao banco de dados
- Dapper - Consulta otimizada ao banco
- MassTransit - Biblioteca para mensageria
- RabbitMQ - Message broker para comunicação assíncrona
- PostgreSQL - Banco de dados relacional
- SmtpClient - Envio de e-mails via SMTP

Requisitos Não Funcionais

**Alta Disponibilidade**

✔️ O sistema deve estar disponível 24/7, garantindo continuidade no atendimento.

Solução:

- Monitoramento & Logs: Integração com Prometheus e Grafana para monitoramento contínuo e alertas proativos
- Orquestração com Docker: O sistema roda em containers Docker, permitindo rápida recuperação e escalabilidade

**Escalabilidade**

✔️ O sistema precisa lidar com até 20.000 usuários simultâneos e suportar aumento na demanda.

Solução:

- RabbitMQ: Processa notificações e envia e-mails de forma assíncrona, reduzindo carga nas APIs.

**Segurança**

✔️ Proteção dos dados sensíveis seguindo as melhores práticas de segurança da informação.

Solução:

- ASP.NET Core Identity + JWT: garante acesso seguro
- Criptografia: senhas armazenadas com hashing
