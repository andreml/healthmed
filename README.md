## Hackathon Fiap - Arquitetura de Sistemas .NET com Azure

A Health&Med, é uma Operadora de Saúde que tem como objetivo facilitar o agendamento de consultas de pacientes de forma online, prática e rápida.
E para os médicos facilitar o controle das agendas com os seus pacientes.

## Autores

Andre Muniz de Lima - [@andreml](https://github.com/andreml) <br/>
Danielly de Mello Araujo - [@Daniellyaraujo](https://github.com/Daniellyaraujo) <br/>
Guilherme Salvioni Taira - [@guilhermeTaira](https://github.com/guilhermeTaira) <br/>
Jonas Felipe Lira da Silva - [@lirajon1988](https://github.com/lirajon1988) <br/>

## Swagger:
**Web App:** https://healthmedfiap.azurewebsites.net/swagger/index.html <br/>
**AKS:** http://135.237.22.73/swagger/index.html

## Stack utilizada

**Back-end:** .Net 7, EF Core, SQL Server, FluentValidation para validações de entrada, XUnit para testes unitários e MimeKit com MailKit para envio de emails.

**Tabelas**

Criamos 4 tabelas para atender os requistos do projeto

**Doctor:** Tabela com os dados do médico <br/>
**Patient:** Tabela com os dados do paciente <br/>
**Appontment:** Tabela das consultas agendadas do paciente <br/>
**Schedule:** Tabela da agenda de atendimento do Médico <br/>

**Diagrama das tabelas:** 

<img src="https://uploaddeimagens.com.br/images/004/819/214/full/Sem_t%C3%ADtulo.png?1722544352">

**Arquitetura do sistema**

Nossa arquitetura está toda em nuvem, disponibilizamos a api no Web App e no AKS do Azure, criamos uma banco de dados também no Azure.

O envio de email aproveitamos o SMTP da Microsoft com o Office365.

<img src="https://uploaddeimagens.com.br/images/004/819/348/full/arquitetura-draw_%281%29.jpg?1722557939">

## Principais funcionalidades

**1. Cadastro do Usuário (Médico)** <br/>
    O médico deverá poder se cadastrar, preenchendo os campos: Nome, CPF, Número CRM, E-mail e Senha.

    Mensagens de retorno:
     Nome é obrigatório
     Nome deve ter entre 3 e 100 caracteres
     Email é obrigatório
     Email deve ter entre 5 e 500 caracteres
     Email inválido
     Cpf é obrigatório
     Cpf inválido
     Senha é obrigatória
     Senha deve conter no mínimo 8 caracteres
     Senha deve ter pelo menos uma letra maiúscula, uma minúscula, um número e um caractere especial
     CRM é obrigatório
     CRM inválido

**2. Autenticação do Usuário (Médico)**<br/>
    O sistema deve permitir que o médico faça login usando o E-mail e uma Senha.<br/>

    Mensagens de retorno:
    Não foi possível gerar token para acesso do usuário.
   
**3. Cadastro/Edição de Horários Disponíveis (Médico)** <br/>
    O sistema deve permitir que o médico faça o Cadastro e Edição de seus horários disponíveis para agendamento de consultas.

    Mensagens de retorno:
    Código é obrigatório
    Hora início é obrigatório
    Hora início deve terminar com minutos 00 ou 30
    Agendas devem no mínimo um intervalo de 30 minutos
    Hora fim é obrigatório
    Hora deve terminar com minutos 00 ou 30
    Uma agenda deve iniciar e finalizar no mesmo dia
    Data início deve ser menor que Data fim
    
**4. Cadastro do Usuário (Paciente)** <br/>
    O paciente poderá se cadastrar preenchendo os campos Nome, CPF, Email e Senha.
    
**5. Autenticação do Usuário (Paciente)** <br/>
    O sistema deve permitir que o paciente faça login usando um E-mail e Senha.

    **Mensagens de retorno:**
    Não foi possível gerar token para acesso do usuário.
    
**6. Busca por Médicos (Paciente)** <br/>
O sistema deve permitir que o paciente visualize a listagem dos médicos
disponíveis.

**7. Agendamento de Consultas (Paciente)** <br/>
    Após selecionar o médico, o paciente deve poder visualizar a agenda do médico com os horários disponíveis e efetuar o agendamento.
   
    Código é obrigatório
    Data início é obrigatório
    São permitidos apenas horários com final 00min e 30min
    Data fim é obrigatório
    São permitidos apenas horários com final 00min e 30min
    Data início deve ser menor que Data fim
    Consultas devem ter 30 minutos de intervalo

**9. Notificação de consulta marcada (Médico)** <br/>
    Após o agendamento, feito pelo usuário Paciente, o médico deverá receber um e-mail contendo:
   
        Título do e-mail:
        ”Health&Med - Nova consulta agendada”
        Corpo do e-mail:
        ”Olá, Dr. {nome_do_médico}!
        Você tem uma nova consulta marcada!
        Paciente: {nome_do_paciente}.
        Data e horário: {data} às {horário_agendado}.

**Requisitos Não Funcionais**

**1. Concorrência de Agendamentos** <br/>
    O sistema deve ser capaz de suportar múltiplos acessos simultâneos e garantir que apenas uma marcação de consulta seja permitida para um determinado horário.
   
**2. Validação de Conflito de Horários** <br/>
O sistema deve validar a disponibilidade do horário selecionado em tempo real, assegurando que não haja sobreposição de horários para consultas agendadas:

Além das validações antes de salvar um horário, criamos uma índice único para evitar que existam duas consultas com o mesmo horário para uma agenda:
[AppointmentMapping.cs](src/HealthMed.Infra/Data/Mapping/AppointmentMapping.cs) - linha 33

Dessa forma, temos um tratamento caso haja uma concorrência e acabe tentando inserir um índice duplicado:
[AppointmentMapping.cs](src/HealthMed.Application/Services/AppointmentService.cs) - linha 76

Para fazer o build desse projeto execute os comandos

```bash
  dotnet restore
```

```bash
  dotnet build
```
## Executando Localmente - API

Clone o projeto

```bash
  git clone https://github.com/andreml/healthmed
```

Abra o Visual Studio e no Package Manager Console entre no diretório do projeto

```bash
   cd .\src\healthmed.Api\
```

Instale as dependências

```bash
  dotnet restore
```

```bash
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 7.0.13
```

Altere o Default project no Package Manager Console para src\HealthMed.Infra

Execute o comando abaixo

```bash
  Update-Database
```

Inicie o servidor

```bash
  dotnet run --project HealthMed.Api.csproj --property:Configuration=Release --port 5133
```

Abra o navegador

```bash
   E digite o endereço http://localhost:5133/swagger/index.html
```

## Executando os testes

Para executar os testes, rode o seguinte comando

```bash
  cd .\test\HealthMed.UnitTests\
```

```bash
  dotnet test HealthMed.UnitTests.csproj --logger "html;logfilename=testResults.html"
```

O teste result ficará salvo na pasta abaixo

```bash
  cd .\test\HealthMed.IntegrationTests\TestResults\
```


## Exemplos válidos para executar as rotas

Os exemplos abaixo estão considerando como domínio rotas em localhost, para execução local. Em outras publicações é necessário fazer ajuste da rota

**POST - /Patient**

Cria uma conta de paciente. Exemplo:

```
curl -X 'POST' \
  'https://localhost:7163/Patient' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "name": "Jonas Lira",
  "cpf": "56061446039",
  "email": "mecawa4964@mfunza.com",
  "password": "Test&123"
}'
```

**POST - /Patient/Auth**

Autentica um paciente. Exemplo:

```
curl -X 'POST' \
  'https://localhost:7163/Patient/Auth' \
  -H 'accept: application/json' \
  -H 'Content-Type: application/json' \
  -d '{
 "email": "mecawa4964@mfunza.com",
  "password": "Test&123"
}'
```

**POST - /Schedule**

Disponibiliza uma agenda (médico). Exemplo:

```
curl -X 'POST' \
  'https://localhost:7163/Schedule' \
  -H 'accept: */*' \
  -H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoiRG9jdG9yIiwiSWQiOiJhNmJkNTVjOS04MGZmLTQ4ZGEtYjI2Zi0wOGRjYjI4ZDExZTIiLCJuYmYiOjE3MjI1NTk4OTEsImV4cCI6MTcyMzE2NDY5MSwiaWF0IjoxNzIyNTU5ODkxfQ.KQr-Zt5xJdbJOJNsH81XttwzaEIKt54o4NWw0aDWj8I' \
  -H 'Content-Type: application/json' \
  -d '{
  "startAvailabilityDate": "2024-08-02T08:00",
  "endAvailabilityDate": "2024-08-02T08:30"
}'
```

**PUT - /Schedule**

Altera uma agenda (médico). Exemplo:

```
curl -X 'PUT' \
  'https://localhost:7163/Schedule' \
  -H 'accept: */*' \
  -H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoiRG9jdG9yIiwiSWQiOiJhNmJkNTVjOS04MGZmLTQ4ZGEtYjI2Zi0wOGRjYjI4ZDExZTIiLCJuYmYiOjE3MjI1NTk4OTEsImV4cCI6MTcyMzE2NDY5MSwiaWF0IjoxNzIyNTU5ODkxfQ.KQr-Zt5xJdbJOJNsH81XttwzaEIKt54o4NWw0aDWj8I' \
  -H 'Content-Type: application/json' \
  -d '{
  "scheduleId": "1E78226C-27DF-4D6F-9A00-08DCB28E2F43",
  "startAvailabilityDate": "2024-08-02T09:00",
  "endAvailabilityDate": "2024-08-02T09:30"
}'
```

**DELETE - /Schedule/{Id}**

Remove uma agenda (médico). Exemplo:

```
curl -X 'DELETE' \
  'https://localhost:7163/Schedule/1E78226C-27DF-4D6F-9A00-08DCB28E2F43' \
  -H 'accept: */*' \
  -H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoiRG9jdG9yIiwiSWQiOiJhNmJkNTVjOS04MGZmLTQ4ZGEtYjI2Zi0wOGRjYjI4ZDExZTIiLCJuYmYiOjE3MjI1NTk4OTEsImV4cCI6MTcyMzE2NDY5MSwiaWF0IjoxNzIyNTU5ODkxfQ.KQr-Zt5xJdbJOJNsH81XttwzaEIKt54o4NWw0aDWj8I'
```

**GET - /Schedule/Doctor/{id}/Available**

Obtém agendas livres de um Médico (Paciente). Exemplo:

```
curl -X 'GET' \
  'https://localhost:7163/Schedule/Doctor/A6BD55C9-80FF-48DA-B26F-08DCB28D11E2/Available?startDate=2024-08-02&endDate=2024-08-02' \
  -H 'accept: application/json' \
  -H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoiUGF0aWVudCIsIklkIjoiMjJkN2NlZDUtZDU0MS00OTM2LTNiNmItMDhkY2IyOGNiZjQyIiwibmJmIjoxNzIyNTU5NzEyLCJleHAiOjE3MjMxNjQ1MTIsImlhdCI6MTcyMjU1OTcxMn0.hPnFScmDhiVBT_WAXPYiC3JyEg1TFXjNDFg3c3dukEI'
```

**GET - /Schedule/Doctor**

Obtém detalhes de agendas de um Médico (Médico). Exemplo:

```
curl -X 'GET' \
  'https://localhost:7163/Schedule/Doctor?startDate=2024-08-02T09%3A30&endDate=2024-08-02T09%3A30' \
  -H 'accept: application/json' \
  -H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoiRG9jdG9yIiwiSWQiOiJhNmJkNTVjOS04MGZmLTQ4ZGEtYjI2Zi0wOGRjYjI4ZDExZTIiLCJuYmYiOjE3MjI1NTk4OTEsImV4cCI6MTcyMzE2NDY5MSwiaWF0IjoxNzIyNTU5ODkxfQ.KQr-Zt5xJdbJOJNsH81XttwzaEIKt54o4NWw0aDWj8I'
```

**POST - /Doctor**

Cria uma conta de médico. Exemplo:

```
curl -X 'POST' \
  'https://localhost:7163/Doctor' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "name": "Jonas Lira",
  "cpf": "97037344052",
  "email": "mecawa4965@mfunza.com",
  "password": "Test&123",
  "crm": "1231234123"
}'
```

**GET - /Doctor**

Consulta uma lista de médicos. Exemplo:

```
curl -X 'GET' \
  'https://localhost:7163/Doctor' \
  -H 'accept: application/json' \
  -H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoiUGF0aWVudCIsIklkIjoiMjJkN2NlZDUtZDU0MS00OTM2LTNiNmItMDhkY2IyOGNiZjQyIiwibmJmIjoxNzIyNTU5NzEyLCJleHAiOjE3MjMxNjQ1MTIsImlhdCI6MTcyMjU1OTcxMn0.hPnFScmDhiVBT_WAXPYiC3JyEg1TFXjNDFg3c3dukEI'
```

**POST - /Doctor/Auth**

Request exemplo:

```
curl -X 'POST' \
  'https://localhost:7163/Doctor/Auth' \
  -H 'accept: application/json' \
  -H 'Content-Type: application/json' \
  -d '{
  "email": "mecawa4965@mfunza.com",
  "password": "Test&123"
}'
```

**POST - /Appointment**

Marca uma consulta (paciente). Exemplo:

```
curl -X 'POST' \
  'https://localhost:7163/Appointment' \
  -H 'accept: */*' \
  -H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoiUGF0aWVudCIsIklkIjoiMjJkN2NlZDUtZDU0MS00OTM2LTNiNmItMDhkY2IyOGNiZjQyIiwibmJmIjoxNzIyNTU5NzEyLCJleHAiOjE3MjMxNjQ1MTIsImlhdCI6MTcyMjU1OTcxMn0.hPnFScmDhiVBT_WAXPYiC3JyEg1TFXjNDFg3c3dukEI' \
  -H 'Content-Type: application/json' \
  -d '{
  "scheduleId": "1E78226C-27DF-4D6F-9A00-08DCB28E2F43",
  "startDate": "2024-08-02T09:00",
  "endDate": "2024-08-02T09:30"
}'
```

**DELETE - /Appointment/{id}**

Desmarca uma consulta (paciente). Exemplo:

```
curl -X 'DELETE' \
  'https://localhost:7163/Appointment/627B6848-0FA4-4689-46A9-08DCB28F2F58' \
  -H 'accept: */*' \
  -H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoiUGF0aWVudCIsIklkIjoiMjJkN2NlZDUtZDU0MS00OTM2LTNiNmItMDhkY2IyOGNiZjQyIiwibmJmIjoxNzIyNTU5NzEyLCJleHAiOjE3MjMxNjQ1MTIsImlhdCI6MTcyMjU1OTcxMn0.hPnFScmDhiVBT_WAXPYiC3JyEg1TFXjNDFg3c3dukEI'
```

**GET - /Appointment/Patient**

Obtem consultas marcadas (paciente). Exemplo:

```
curl -X 'GET' \
  'https://localhost:7163/Appointment/Patient?startDate=2024-08-02T00%3A00&endDate=2024-08-03T00%3A00' \
  -H 'accept: application/json' \
  -H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoiUGF0aWVudCIsIklkIjoiMjJkN2NlZDUtZDU0MS00OTM2LTNiNmItMDhkY2IyOGNiZjQyIiwibmJmIjoxNzIyNTU5NzEyLCJleHAiOjE3MjMxNjQ1MTIsImlhdCI6MTcyMjU1OTcxMn0.hPnFScmDhiVBT_WAXPYiC3JyEg1TFXjNDFg3c3dukEI'
  ```
