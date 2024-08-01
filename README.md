## Hackatoon Fiap - Arquitetura de Sistemas .NET com Azure

A Health&Med, é uma Operadora de Saúde que tem como objetivo facilitar o agendamento de consultas de pacientes de forma online, prática e rápida.
E para os médicos facilitar o controle das agendas com os seus pacientes.

## Autores

- [@guilhermeTaira](https://github.com/guilhermeTaira)
- [@lirajon1988](https://github.com/lirajon1988)
- [@andreml](https://github.com/andreml)
- [@Daniellyaraujo](https://github.com/Daniellyaraujo)

## Stack utilizada

**Back-end:** .Net 7, EF Core, FluentValidation e XUnit

**Tabelas**

Criamos 4 tabelas para atender os requistos do projeto

**Doctor:** Tabela com os dados do médico
**Patient:** Tabela com os dados do paciente
**Appontment:** Tabela das consultas agendadas do paciente
**Schedule:** Tabela da agenda de atendimento do Médico

**Diagrama das tabelas:** 

<img src="https://uploaddeimagens.com.br/images/004/819/214/full/Sem_t%C3%ADtulo.png?1722544352">

**Requisitos Funcionais**

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
    DoctorId é obrigatório
    StartAvailabilityDate é obrigatório
    StartAvailabilityDate deve terminar com minutos 00 ou 30
    Agendas devem no mínimo um intervalo de 30 minutos
    EndAvailabilityDate é obrigatório
    EndAvailabilityDate deve terminar com minutos 00 ou 30
    Uma agenda deve iniciar e finalizar no mesmo dia
    StartAvailabilityDate deve ser menor que EndAvailabilityDate
    
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
   
    ScheduleId é obrigatório
    StartDate é obrigatório
    São permitidos apenas horários com final 00min e 30min
    EndDate é obrigatório
    São permitidos apenas horários com final 00min e 30min
    StartDate deve ser menor que EndDate
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
O sistema deve validar a disponibilidade do horário selecionado em tempo real, assegurando que não haja sobreposição de horários para consultas agendadas.

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
