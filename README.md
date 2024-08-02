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

    Validações:
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
    
Request exemplo:
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

---

**2. Autenticação do Usuário (Médico)**<br/>
    O sistema deve permitir que o médico faça login usando o E-mail e uma Senha.<br/>

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

---
   
**3. Cadastro/Edição de Horários Disponíveis (Médico)** <br/>
    O sistema deve permitir que o médico faça o Cadastro e Edição de seus horários disponíveis para agendamento de consultas.

    Validações:
        Código é obrigatório
        Hora início é obrigatório
        Hora início deve terminar com minutos 00 ou 30
        Agendas devem no mínimo um intervalo de 30 minutos
        Hora fim é obrigatório
        Hora deve terminar com minutos 00 ou 30
        Uma agenda deve iniciar e finalizar no mesmo dia
        Data início deve ser menor que Data fim

Request exemplo cadastro de horários:
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

Request exemplo edição de horários:
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

---
    
**4. Cadastro do Usuário (Paciente)** <br/>
    O paciente poderá se cadastrar preenchendo os campos Nome, CPF, Email e Senha.

    Validações:
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

Request exemplo:
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

---
    
**5. Autenticação do Usuário (Paciente)** <br/>
    O sistema deve permitir que o paciente faça login usando um E-mail e Senha.

Request exemplo:
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

---

    
**6. Busca por Médicos (Paciente)** <br/>
O sistema deve permitir que o paciente visualize a listagem dos médicos
disponíveis.

Request exemplo:
```
curl -X 'GET' \
  'https://localhost:7163/Doctor' \
  -H 'accept: application/json' \
  -H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoiUGF0aWVudCIsIklkIjoiMjJkN2NlZDUtZDU0MS00OTM2LTNiNmItMDhkY2IyOGNiZjQyIiwibmJmIjoxNzIyNTU5NzEyLCJleHAiOjE3MjMxNjQ1MTIsImlhdCI6MTcyMjU1OTcxMn0.hPnFScmDhiVBT_WAXPYiC3JyEg1TFXjNDFg3c3dukEI'
```

---

**7. Agendamento de Consultas (Paciente)** <br/>
    Após selecionar o médico, o paciente deve poder visualizar a agenda do médico com os horários disponíveis e efetuar o agendamento.
   
    Código é obrigatório
    Data início é obrigatório
    São permitidos apenas horários com final 00min e 30min
    Data fim é obrigatório
    São permitidos apenas horários com final 00min e 30min
    Data início deve ser menor que Data fim
    Consultas devem ter 30 minutos de intervalo

Request exemplo:
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

---

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

## Outros Exemplos de requisições:

Os exemplos abaixo estão considerando como domínio rotas em localhost, para execução local. Em outras publicações é necessário fazer ajuste da rota


**DELETE - /Schedule/{Id}**

Remove uma agenda (médico). Exemplo:

```
curl -X 'DELETE' \
  'https://localhost:7163/Schedule/1E78226C-27DF-4D6F-9A00-08DCB28E2F43' \
  -H 'accept: */*' \
  -H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoiRG9jdG9yIiwiSWQiOiJhNmJkNTVjOS04MGZmLTQ4ZGEtYjI2Zi0wOGRjYjI4ZDExZTIiLCJuYmYiOjE3MjI1NTk4OTEsImV4cCI6MTcyMzE2NDY5MSwiaWF0IjoxNzIyNTU5ODkxfQ.KQr-Zt5xJdbJOJNsH81XttwzaEIKt54o4NWw0aDWj8I'
```

**GET - /Schedule/Doctor**

Possibilita o médico visualizar sua agenda, com horários e consultas marcadas. Exemplo:

```
curl -X 'GET' \
  'https://localhost:7163/Schedule/Doctor?startDate=2024-08-02T09%3A30&endDate=2024-08-02T09%3A30' \
  -H 'accept: application/json' \
  -H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoiRG9jdG9yIiwiSWQiOiJhNmJkNTVjOS04MGZmLTQ4ZGEtYjI2Zi0wOGRjYjI4ZDExZTIiLCJuYmYiOjE3MjI1NTk4OTEsImV4cCI6MTcyMzE2NDY5MSwiaWF0IjoxNzIyNTU5ODkxfQ.KQr-Zt5xJdbJOJNsH81XttwzaEIKt54o4NWw0aDWj8I'
```

**DELETE - /Appointment/{id}**

Permite que o cliente desmarque uma consulta. Exemplo:

```
curl -X 'DELETE' \
  'https://localhost:7163/Appointment/627B6848-0FA4-4689-46A9-08DCB28F2F58' \
  -H 'accept: */*' \
  -H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoiUGF0aWVudCIsIklkIjoiMjJkN2NlZDUtZDU0MS00OTM2LTNiNmItMDhkY2IyOGNiZjQyIiwibmJmIjoxNzIyNTU5NzEyLCJleHAiOjE3MjMxNjQ1MTIsImlhdCI6MTcyMjU1OTcxMn0.hPnFScmDhiVBT_WAXPYiC3JyEg1TFXjNDFg3c3dukEI'
```

**GET - /Appointment/Patient**

Permite o cliente visualizar suas consultas agendadas. Exemplo:

```
curl -X 'GET' \
  'https://localhost:7163/Appointment/Patient?startDate=2024-08-02T00%3A00&endDate=2024-08-03T00%3A00' \
  -H 'accept: application/json' \
  -H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoiUGF0aWVudCIsIklkIjoiMjJkN2NlZDUtZDU0MS00OTM2LTNiNmItMDhkY2IyOGNiZjQyIiwibmJmIjoxNzIyNTU5NzEyLCJleHAiOjE3MjMxNjQ1MTIsImlhdCI6MTcyMjU1OTcxMn0.hPnFScmDhiVBT_WAXPYiC3JyEg1TFXjNDFg3c3dukEI'
  ```

## Build do projeto

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

## COLLECTION POSTMAN


{
	"info": {
		"_postman_id": "004970cf-9621-478e-a4fe-cca54846b506",
		"name": "Health & Med",
		"schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json",
		"_exporter_id": "11189715"
	},
	"item": [
		{
			"name": "Paciente",
			"item": [
				{
					"name": "Cadastro do Usuário (Paciente)",
					"request": {
						"method": "POST",
						"header": [],
						"body": {
							"mode": "raw",
							"raw": "{\r\n    \"name\": \"Woody Woodpecker\",\r\n    \"cpf\": \"03609924047\",\r\n    \"email\": \"woodywoodpecker@gmail.com\",\r\n    \"password\": \"Test&123\"\r\n}",
							"options": {
								"raw": {
									"language": "json"
								}
							}
						},
						"url": {
							"raw": "{{URL_HEALTH}}/Patient",
							"host": [
								"{{URL_HEALTH}}"
							],
							"path": [
								"Patient"
							]
						}
					},
					"response": []
				},
				{
					"name": "Autenticação do Usuário (Paciente)",
					"event": [
						{
							"listen": "test",
							"script": {
								"exec": [
									"pm.globals.set(\"TOKEN_PATIENT\", pm.response.json().token)"
								],
								"type": "text/javascript",
								"packages": {}
							}
						}
					],
					"request": {
						"method": "POST",
						"header": [],
						"body": {
							"mode": "raw",
							"raw": "{\r\n \"email\": \"woodywoodpecker@gmail.com\",\r\n  \"password\": \"Test&123\"\r\n}",
							"options": {
								"raw": {
									"language": "json"
								}
							}
						},
						"url": {
							"raw": "{{URL_HEALTH}}/Patient/Auth",
							"host": [
								"{{URL_HEALTH}}"
							],
							"path": [
								"Patient",
								"Auth"
							]
						}
					},
					"response": []
				}
			]
		},
		{
			"name": "Médico",
			"item": [
				{
					"name": "Cadastro do Usuário (Médico)",
					"request": {
						"method": "POST",
						"header": [],
						"body": {
							"mode": "raw",
							"raw": "{\r\n  \"name\": \"Hans Chucrutes\",\r\n  \"cpf\": \"64440592082\",\r\n  \"email\": \"wirow14358@mvpalace.com\",\r\n  \"password\": \"Test&123\",\r\n  \"crm\": \"644405\"\r\n}",
							"options": {
								"raw": {
									"language": "json"
								}
							}
						},
						"url": {
							"raw": "{{URL_HEALTH}}/Doctor",
							"host": [
								"{{URL_HEALTH}}"
							],
							"path": [
								"Doctor"
							]
						}
					},
					"response": []
				},
				{
					"name": "Autenticação do Usuário (Médico)",
					"event": [
						{
							"listen": "test",
							"script": {
								"exec": [
									"pm.globals.set(\"TOKEN_DOCTOR\", pm.response.json().token)"
								],
								"type": "text/javascript",
								"packages": {}
							}
						},
						{
							"listen": "prerequest",
							"script": {
								"exec": [
									""
								],
								"type": "text/javascript",
								"packages": {}
							}
						}
					],
					"request": {
						"method": "POST",
						"header": [],
						"body": {
							"mode": "raw",
							"raw": "{\r\n  \"email\": \"wirow14358@mvpalace.com\",\r\n  \"password\": \"Test&123\"\r\n}",
							"options": {
								"raw": {
									"language": "json"
								}
							}
						},
						"url": {
							"raw": "{{URL_HEALTH}}/Doctor/Auth",
							"host": [
								"{{URL_HEALTH}}"
							],
							"path": [
								"Doctor",
								"Auth"
							]
						}
					},
					"response": []
				},
				{
					"name": "Busca por Médicos (Paciente)",
					"request": {
						"auth": {
							"type": "bearer",
							"bearer": [
								{
									"key": "token",
									"value": "{{TOKEN_PATIENT}}",
									"type": "string"
								}
							]
						},
						"method": "GET",
						"header": [],
						"url": {
							"raw": "{{URL_HEALTH}}/Doctor",
							"host": [
								"{{URL_HEALTH}}"
							],
							"path": [
								"Doctor"
							]
						}
					},
					"response": []
				}
			]
		},
		{
			"name": "Agenda",
			"item": [
				{
					"name": "Disponbiliza uma agenda (Médico)",
					"event": [
						{
							"listen": "test",
							"script": {
								"exec": [
									""
								],
								"type": "text/javascript",
								"packages": {}
							}
						}
					],
					"request": {
						"auth": {
							"type": "bearer",
							"bearer": [
								{
									"key": "token",
									"value": "{{TOKEN_DOCTOR}}",
									"type": "string"
								}
							]
						},
						"method": "POST",
						"header": [],
						"body": {
							"mode": "raw",
							"raw": "{\r\n    \"startAvailabilityDate\": \"2024-08-03T08:00\",\r\n    \"endAvailabilityDate\": \"2024-08-03T12:30\"\r\n}",
							"options": {
								"raw": {
									"language": "json"
								}
							}
						},
						"url": {
							"raw": "{{URL_HEALTH}}/Schedule",
							"host": [
								"{{URL_HEALTH}}"
							],
							"path": [
								"Schedule"
							]
						}
					},
					"response": []
				},
				{
					"name": "Obtém detalhes de agendas de um Médico (Médico)",
					"event": [
						{
							"listen": "test",
							"script": {
								"exec": [
									""
								],
								"type": "text/javascript",
								"packages": {}
							}
						}
					],
					"request": {
						"auth": {
							"type": "bearer",
							"bearer": [
								{
									"key": "token",
									"value": "{{TOKEN_DOCTOR}}",
									"type": "string"
								}
							]
						},
						"method": "GET",
						"header": [],
						"url": {
							"raw": "{{URL_HEALTH}}/Schedule/Doctor?startDate=2024-08-03T06%3A00&endDate=2024-08-04T22%3A00",
							"host": [
								"{{URL_HEALTH}}"
							],
							"path": [
								"Schedule",
								"Doctor"
							],
							"query": [
								{
									"key": "startDate",
									"value": "2024-08-03T06%3A00"
								},
								{
									"key": "endDate",
									"value": "2024-08-04T22%3A00"
								}
							]
						}
					},
					"response": []
				},
				{
					"name": "Obtém agendas livres de um Médico (Paciente)",
					"request": {
						"auth": {
							"type": "bearer",
							"bearer": [
								{
									"key": "token",
									"value": "{{TOKEN_PATIENT}}",
									"type": "string"
								}
							]
						},
						"method": "GET",
						"header": [],
						"url": {
							"raw": "{{URL_HEALTH}}/Schedule/Doctor/559914b3-d422-49b4-06b8-08dcb34a68a7/Available?startDate=2024-08-02T06%3A00&endDate=2024-08-05T22%3A00",
							"host": [
								"{{URL_HEALTH}}"
							],
							"path": [
								"Schedule",
								"Doctor",
								"559914b3-d422-49b4-06b8-08dcb34a68a7",
								"Available"
							],
							"query": [
								{
									"key": "startDate",
									"value": "2024-08-02T06%3A00"
								},
								{
									"key": "endDate",
									"value": "2024-08-05T22%3A00"
								}
							]
						}
					},
					"response": []
				},
				{
					"name": "Altera uma agenda do médico",
					"request": {
						"auth": {
							"type": "bearer",
							"bearer": [
								{
									"key": "token",
									"value": "{{TOKEN_DOCTOR}}",
									"type": "string"
								}
							]
						},
						"method": "PUT",
						"header": [],
						"body": {
							"mode": "raw",
							"raw": "{\r\n    \"scheduleId\": \"cebc65cb-1188-401a-cf9f-08dcb33d7dee\",\r\n    \"startAvailabilityDate\": \"2024-08-03T22:00\",\r\n    \"endAvailabilityDate\": \"2024-08-03T22:30\"\r\n}",
							"options": {
								"raw": {
									"language": "json"
								}
							}
						},
						"url": {
							"raw": "{{URL_HEALTH}}/Schedule",
							"host": [
								"{{URL_HEALTH}}"
							],
							"path": [
								"Schedule"
							]
						}
					},
					"response": []
				},
				{
					"name": "Remove uma agenda (Médico)",
					"request": {
						"auth": {
							"type": "bearer",
							"bearer": [
								{
									"key": "token",
									"value": "{{TOKEN_DOCTOR}}",
									"type": "string"
								}
							]
						},
						"method": "DELETE",
						"header": [],
						"url": {
							"raw": "{{URL_HEALTH}}/Schedule/693d87c2-907d-4ebe-3d63-08dcb34a7fd8",
							"host": [
								"{{URL_HEALTH}}"
							],
							"path": [
								"Schedule",
								"693d87c2-907d-4ebe-3d63-08dcb34a7fd8"
							]
						}
					},
					"response": []
				}
			]
		},
		{
			"name": "Consultas",
			"item": [
				{
					"name": "Agendamento de Consultas (Paciente)",
					"request": {
						"auth": {
							"type": "bearer",
							"bearer": [
								{
									"key": "token",
									"value": "{{TOKEN_PATIENT}}",
									"type": "string"
								}
							]
						},
						"method": "POST",
						"header": [],
						"body": {
							"mode": "raw",
							"raw": "{\r\n    \"scheduleId\": \"693d87c2-907d-4ebe-3d63-08dcb34a7fd8\",\r\n    \"startDate\": \"2024-08-03T09:00\",\r\n    \"endDate\": \"2024-08-03T09:30\"\r\n}",
							"options": {
								"raw": {
									"language": "json"
								}
							}
						},
						"url": {
							"raw": "{{URL_HEALTH}}/Appointment",
							"host": [
								"{{URL_HEALTH}}"
							],
							"path": [
								"Appointment"
							]
						}
					},
					"response": []
				},
				{
					"name": "Obtém consultas marcadas (Paciente)",
					"request": {
						"auth": {
							"type": "bearer",
							"bearer": [
								{
									"key": "token",
									"value": "{{TOKEN_PATIENT}}",
									"type": "string"
								}
							]
						},
						"method": "GET",
						"header": [],
						"url": {
							"raw": "{{URL_HEALTH}}/Appointment/Patient?startDate=2024-08-03T06%3A00&endDate=2024-08-03T18%3A00",
							"host": [
								"{{URL_HEALTH}}"
							],
							"path": [
								"Appointment",
								"Patient"
							],
							"query": [
								{
									"key": "startDate",
									"value": "2024-08-03T06%3A00"
								},
								{
									"key": "endDate",
									"value": "2024-08-03T18%3A00"
								}
							]
						}
					},
					"response": []
				},
				{
					"name": "Marca uma consulta (Paciente)",
					"request": {
						"auth": {
							"type": "bearer",
							"bearer": [
								{
									"key": "token",
									"value": "{{TOKEN_PATIENT}}",
									"type": "string"
								}
							]
						},
						"method": "DELETE",
						"header": [],
						"body": {
							"mode": "raw",
							"raw": "{\r\n  \"scheduleId\": \"693d87c2-907d-4ebe-3d63-08dcb34a7fd8\",\r\n  \"startDate\": \"2024-08-02T09:00\",\r\n  \"endDate\": \"2024-08-02T09:30\"\r\n}",
							"options": {
								"raw": {
									"language": "json"
								}
							}
						},
						"url": {
							"raw": "{{URL_HEALTH}}/Appointment/8ae23b8f-90f6-4f35-cf9b-08dcb33d7dee",
							"host": [
								"{{URL_HEALTH}}"
							],
							"path": [
								"Appointment",
								"8ae23b8f-90f6-4f35-cf9b-08dcb33d7dee"
							]
						}
					},
					"response": []
				}
			]
		}
	],
	"event": [
		{
			"listen": "prerequest",
			"script": {
				"type": "text/javascript",
				"packages": {},
				"exec": [
					""
				]
			}
		},
		{
			"listen": "test",
			"script": {
				"type": "text/javascript",
				"packages": {},
				"exec": [
					""
				]
			}
		}
	],
	"variable": [
		{
			"key": "URL_HEALTH",
			"value": "https://healthmedfiap.azurewebsites.net",
			"type": "string"
		}
	]
}
