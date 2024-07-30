﻿using HealthMed.Application.Dtos;
using HealthMed.Application.Services.Interfaces;
using HealthMed.Application.ViewModels;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Enum;
using HealthMed.Domain.Repository;
using HealthMed.Domain.Utils;
using Microsoft.Extensions.Configuration;
using System.Net;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace HealthMed.Application.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repository;
        private readonly IConfiguration _config;

        public DoctorService(IDoctorRepository repository, IConfiguration config)
        {
            _repository = repository;
            _config = config;
        }
        public async Task<ResponseBase> AddDoctorAsync(AddDoctorDto dto)
        {
            var response = new ResponseBase();

            var existingPatient = await _repository.GetByEmailOrCpfAsync(dto.Email, dto.Cpf);

            if (existingPatient is not null)
            {
                response.AddError("Já existe um cadastro de médico com este email ou cpf");
                return response;
            }

            var doctor = new Doctor(dto.Name, dto.Cpf, dto.CRM, dto.Email, dto.Password);

            await _repository.AddAsync(doctor);

            response.AddData("Médico adicionado com sucesso!");

            return response;
        }

        public async Task<ResponseBase> AuthDoctorAsync(AuthDto dto)
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

        public async Task<ResponseBase> GetDoctors()
        {
            var response = new ResponseBase();

            var doctors = await _repository.GetAll();

            if (doctors is null)
            {
                response.AddError("Não há médicos cadastrados!");
            }
            else
            {
                response.AddData(doctors, HttpStatusCode.OK);
            }

            return response;
        }
    }
}