**Health&Med** - Agendamento de Consultas Médicas é um sistema que permite o agendamento de consultas médicas, enviando notificações por e-mail para médicos e pacientes.

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

**_Requisitos Não Funcionais_**

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

**ARQUITETURA**

_Para melhor visualização dos diagramas, instale a extensão draw.io no VSCode e acesse: Documentacao/Arquitetura.drawio_

**Diagrama de Contexto**

![image](https://github.com/user-attachments/assets/a87ad969-9d6e-4ccf-8830-e1e09db97e7c)

**Diagrama de Containers**

![image](https://github.com/user-attachments/assets/b6173b1e-f154-49b0-a68f-59ee70f0ea93)

**Diagrama de Componentes**

- Container de Autenticação

![image](https://github.com/user-attachments/assets/6ac1771d-0733-40b4-bc8a-daff3af38064)


- Container da API de Agendamento de Consultas

![image](https://github.com/user-attachments/assets/c8a4482f-1f9c-4c3f-8cd2-e20bf240cf78)

- Container do Consumidor de Notificações

![image](https://github.com/user-attachments/assets/3a2bbbf2-5c3c-4513-a519-52e9ec3a4aaa)


