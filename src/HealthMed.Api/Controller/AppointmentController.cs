using HealthMed.Application.Dtos;
using HealthMed.Application.Services.Interfaces;
using HealthMed.Application.ViewModels;
using HealthMed.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HealthMed.Api.Controller;

/// <summary>
/// Controller responsável pelo gerenciamento de Consultas
/// </summary>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class AppointmentController : BaseController
{
    readonly IAppointmentService _appointmentService;

    public AppointmentController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    /// <summary>
    /// Marca uma Consulta (Paciente)
    /// </summary>
    [HttpPost]
    //[Authorize(Roles = $"{Perfis.Patient}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddAppointmentAsync(AddAppointmentDto dto)
    {
        dto.PatientId = GetLoggedUserId();

        var result = await _appointmentService.AddAppointmentAsync(dto);

        return Response(result!);
    }

    /// <summary>
    /// Desmarca uma Consulta (Paciente)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = $"{Perfis.Patient}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAppointmentAsync(Guid id)
    {
        var result = await _appointmentService.DeleteAppointmentAsync(GetLoggedUserId(), id);

        return Response(result!);
    }

    /// <summary>
    /// Obtém Consultas marcadas (Paciente)
    /// </summary>
    [HttpGet("Patient")]
    [Authorize(Roles = $"{Perfis.Patient}")]
    [ProducesResponseType(typeof(PatientAppointmentsViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetPatientAppointmentsAsync([FromQuery][Required] DateTime startDate, [FromQuery][Required] DateTime endDate)
    {
        var result = await _appointmentService.GetPatientAppointmentsAsync(GetLoggedUserId(), startDate, endDate);

        return Response(result!);
    }

    /// <summary>
    /// Obtém Consultas marcadas (Médico)
    /// </summary>
    [HttpGet("Doctor")]
    [Authorize(Roles = $"{Perfis.Doctor}")]
    [ProducesResponseType(typeof(DoctorAppointmentsViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetDoctorAppointmentsAsync([FromQuery][Required] DateTime startDate, [FromQuery][Required] DateTime endDate)
    {
        var result = await _appointmentService.GetPatientAppointmentsAsync(GetLoggedUserId(), startDate, endDate);

        return Response(result!);
    }
}
