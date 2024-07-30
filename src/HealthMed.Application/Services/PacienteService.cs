using HealthMed.Application.Dtos;
using HealthMed.Application.Services.Interfaces;
using HealthMed.Application.ViewModels;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Enum;
using HealthMed.Domain.Repository;
using HealthMed.Domain.Utils;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace HealthMed.Application.Services;

public class PacienteService : IPatientService
{
    private readonly IPatientrepository _repository;
    private readonly IConfiguration _config;

    public PacienteService(IPatientrepository repository, IConfiguration config)
    {
        _repository = repository;
        _config = config;
    }

    public async Task<ResponseBase> AddPatientAsync(AddPatientDto dto)
    {
        var response = new ResponseBase();

        var existingPatient = await _repository.GetByEmailOrCpfAsync(dto.Email, dto.Cpf);

        if(existingPatient is not null)
        {
            response.AddError("Já existe um cadastro de paciente com este email ou cpf");
            return response;
        }

        var patient = new Patient(dto.Name, dto.Cpf, dto.Email, dto.Password);

        await _repository.AddAsync(patient);

        response.AddData("Paciente adicionado com sucesso!");

        return response;
    }

    public async Task<ResponseBase> AuthPatientAsync(AuthDto dto)
    {
        var response = new ResponseBase();

        var patient = await _repository.GetByEmailAndPasswordAsync(dto.Email, dto.Password);

        if (patient is null)
        {
            response.AddError("Não foi possível gerar token para acesso do usuário"); 
        }
        else
        {
            var login = new LoginViewModel(
                                        patient.Name,
                                        patient.Email,
                                        Profile.Patient,
                                        JwtUtils.GenerateToken(patient.Id, Profile.Patient, DateTime.Now.AddDays(7), _config));

            response.AddData(login, HttpStatusCode.OK);
        }

        return response;
    }
}
