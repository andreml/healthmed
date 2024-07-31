using HealthMed.Application.Dtos;
using HealthMed.Application.Services.Interfaces;
using HealthMed.Application.ViewModels;
using HealthMed.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HealthMed.Api.Controller;

/// <summary>
/// Gerenciamento de Agendas
/// </summary>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class ScheduleController : BaseController
{
    readonly IScheduleService _scheduleService;

    public ScheduleController(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    /// <summary>
    /// Disponibiliza uma agenda (Médico)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = $"{Perfis.Doctor}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddScheduleAsync(AddScheduleDto dto)
    {
        dto.DoctorId = GetLoggedUserId();

        var result = await _scheduleService.AddScheduleAsync(dto);

        return Response(result!);
    }

    /// <summary>
    /// Altera uma agenda (Médico)
    /// </summary>
    [HttpPut]
    [Authorize(Roles = $"{Perfis.Doctor}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateScheduleAsync(UpdateScheduleDto dto)
    {
        dto.DoctorId = GetLoggedUserId();

        var result = await _scheduleService.UpdateScheduleAsync(dto);

        return Response(result!);
    }

    /// <summary>
    /// Remove uma agenda (Médico)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = $"{Perfis.Doctor}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteScheduleAsync(Guid id)
    {
        var result = await _scheduleService.DeleteScheduleAsync(GetLoggedUserId(), id);

        return Response(result!);
    }

    /// <summary>
    /// Obtém agendas livres de um Médico (Paciente)
    /// </summary>
    [HttpGet("Doctor/{id}/Available")]
    [Authorize(Roles = $"{Perfis.Patient}")]
    [ProducesResponseType( typeof(AvailableSchedulesViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType( StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAvailableSchedulesAsync([FromRoute]Guid id, [FromQuery][Required] DateTime startDate, [FromQuery][Required] DateTime endDate)
    {
        var result = await _scheduleService.GetAvailableSchedulesAsync(id, startDate, endDate);

        return Response(result!);
    }

    /// <summary>
    /// Obtém detalhes de agendas de um Médico (Médico)
    /// </summary>
    [HttpGet("Doctor")]
    [Authorize(Roles = $"{Perfis.Doctor}")]
    [ProducesResponseType(typeof(ICollection<ScheduleViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetSchedulesAsync([FromQuery][Required] DateTime startDate, [FromQuery][Required] DateTime endDate)
    {
        var result = await _scheduleService.GetSchedulesAsync(GetLoggedUserId(), startDate, endDate);

        return Response(result!);
    }
}
