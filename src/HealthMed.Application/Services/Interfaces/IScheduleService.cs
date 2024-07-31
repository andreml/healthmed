﻿using HealthMed.Application.Dtos;
using HealthMed.Application.ViewModels;

namespace HealthMed.Application.Services.Interfaces;

public interface IScheduleService
{
    Task<ResponseBase> AddScheduleAsync(AddScheduleDto dto);
    Task<ResponseBase> UpdateScheduleAsync(UpdateScheduleDto dto);
    Task<ResponseBase> DeleteScheduleAsync(Guid doctorId, Guid scheduleId);
}
