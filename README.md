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

**Diagrama das tabelas:** 

**Requisitos Funcionais**
**1. Cadastro do Usuário (Médico)**<br/>
    O médico deverá poder se cadastrar, preenchendo os campos abaixo<br/>
    **Obrigatórios:** Nome, CPF, Número CRM, E-mail e Senha.

    **Mensagens de retorno:**<br/>
     Nome é obrigatório<br/>
     Nome deve ter entre 3 e 100 caracteres<br/>
     Email é obrigatório<br/>
     Email deve ter entre 5 e 500 caracteres<br/>
     Email inválido<br/>
     Cpf é obrigatório<br/>
     Cpf inválido<br/>
     Senha é obrigatória<br/>
     Senha deve conter no mínimo 8 caracteres<br/>
     Senha deve ter pelo menos uma letra maiúscula, uma minúscula, um número e um caractere especial<br/>
     CRM é obrigatório
     CRM inválido

**2. Autenticação do Usuário (Médico)**<br/>
    O sistema deve permitir que o médico faça login usando o E-mail e uma Senha.<br/>
    Obrigatórios: E-mail e Senha.<br/>

    **Mensagens de retorno:**<br/>
    Não foi possível gerar token para acesso do usuário.<br/>
   
**3. Cadastro/Edição de Horários Disponíveis (Médico)**<br/>
    O sistema deve permitir que o médico faça o Cadastro e Edição de seus horários disponíveis para agendamento de consultas.

    **Mensagens de retorno:**<br/>
    DoctorId é obrigatório<br/>
    StartAvailabilityDate é obrigatório<br/>
    StartAvailabilityDate deve terminar com minutos 00 ou 30<br/>
    Agendas devem no mínimo um intervalo de 30 minutos<br/>
    EndAvailabilityDate é obrigatório<br/>
    EndAvailabilityDate deve terminar com minutos 00 ou 30<br/>
    Uma agenda deve iniciar e finalizar no mesmo dia<br/>
    StartAvailabilityDate deve ser menor que EndAvailabilityDate<br/>
    
4. Cadastro do Usuário (Paciente)
    O paciente poderá se cadastrar preenchendo os campos abaixo
    Nome, CPF, Email e Senha.
6. Autenticação do Usuário (Paciente)
O sistema deve permitir que o paciente faça login usando um E-mail e
Senha.
7. Busca por Médicos (Paciente)
O sistema deve permitir que o paciente visualize a listagem dos médicos
disponíveis.
8. Agendamento de Consultas (Paciente)
Após selecionar o médico, o paciente deve poder visualizar a agenda do
médico com os horários disponíveis e efetuar o agendamento.
9. Notificação de consulta marcada (Médico)
Após o agendamento, feito pelo usuário Paciente, o médico deverá
receber um e-mail contendo:
Título do e-mail:
”Health&Med - Nova consulta agendada”
Corpo do e-mail:
”Olá, Dr. {nome_do_médico}!
Você tem uma nova consulta marcada!
Paciente: {nome_do_paciente}.
Data e horário: {data} às {horário_agendado}.”
Hackathon - Turma .NET 3
Requisitos Não Funcionais
1. Concorrência de Agendamentos
O sistema deve ser capaz de suportar múltiplos acessos simultâneos e
garantir que apenas uma marcação de consulta seja permitida para um
determinado horário.
2. Validação de Conflito de Horários
O sistema deve validar a disponibilidade do horário selecionado em tempo
real, assegurando que não haja sobreposição de horários para consultas
agendadas.

Para fazer o build desse projeto rode

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
