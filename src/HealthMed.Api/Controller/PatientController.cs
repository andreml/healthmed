using HealthMed.Application.Dtos;
using HealthMed.Application.Services.Interfaces;
using HealthMed.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.Api.Controller;

/// <summary>
/// Gerenciamento dos Pacientes
/// </summary>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class PatientController : BaseController
{
    readonly IPatientService _patientService;
    public PatientController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    /// <summary>
    /// Cria uma conta de Paciente
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddPatientAsync(AddPatientDto dto)
    {
        var result = await _patientService.AddPatientAsync(dto);

        return Response(result!);
    }

    /// <summary>
    /// Autentica um Paciente
    /// </summary>
    [HttpPost("Auth")]
    [ProducesResponseType(typeof(LoginViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AuthPatientAsync(AuthDto dto)
    {
        var result = await _patientService.AuthPatientAsync(dto);

        return Response(result!);
    }
}
