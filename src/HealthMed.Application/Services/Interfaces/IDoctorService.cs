using HealthMed.Application.Dtos;
using HealthMed.Application.ViewModels;

namespace HealthMed.Application.Services.Interfaces;

public interface IDoctorService
{
    Task<ResponseBase> AddDoctorAsync(AddDoctorDto dto);
    Task<ResponseBase> AuthDoctorAsync(AuthDto dto);
    Task<ResponseBase> GetDoctors();
}
