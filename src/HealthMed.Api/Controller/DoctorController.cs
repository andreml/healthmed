using HealthMed.Application.Dtos;
using HealthMed.Application.Services.Interfaces;
using HealthMed.Application.ViewModels;
using HealthMed.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.Api.Controller;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class DoctorController : BaseController
{
    readonly IDoctorService _doctorService;

    public DoctorController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    /// <summary>
    /// Cria uma conta de Médico
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddPatientAsync(AddDoctorDto dto)
    {
        var result = await _doctorService.AddDoctorAsync(dto);

        return Response(result!);
    }

    /// <summary>
    /// Autentica um Médico
    /// </summary>
    [HttpPost("Auth")]
    [ProducesResponseType(typeof(LoginViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AuthPatientAsync(AuthDto dto)
    {
        var result = await _doctorService.AuthDoctorAsync(dto);

        return Response(result!);
    }

    /// <summary>
    /// Consulta uma lista de médicos
    /// </summary>
    [HttpGet()]
    [Authorize(Roles = $"{Perfis.Patient}")]
    [ProducesResponseType(typeof(List<DoctorViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Get()
    {
        var result = await _doctorService.GetDoctors();

        return Response(result!);
    }
}