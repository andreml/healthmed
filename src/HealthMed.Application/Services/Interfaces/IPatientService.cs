using HealthMed.Application.Dtos;
using HealthMed.Application.ViewModels;

namespace HealthMed.Application.Services.Interfaces;

public interface IPatientService
{
    Task<ResponseBase> AddPatientAsync(AddPatientDto dto);
    Task<ResponseBase> AuthPatientAsync(AuthDto dto);
}
