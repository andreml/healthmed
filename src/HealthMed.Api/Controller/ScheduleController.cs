using HealthMed.Application.Dtos;
using HealthMed.Application.Services.Interfaces;
using HealthMed.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.Api.Controller;

/// <summary>
/// Controller responsável pelo gerenciamento de Agendas
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
}
