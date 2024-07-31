using HealthMed.Application.Dtos;
using HealthMed.Application.Services.Interfaces;
using HealthMed.Application.ViewModels;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Repository;

namespace HealthMed.Application.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IAppointmentRepository _appointmentRepository;

        public ScheduleService(
                        IDoctorRepository doctorRepository, 
                        IScheduleRepository scheduleRepository, 
                        IAppointmentRepository appointmentRepository)
        {
            _doctorRepository = doctorRepository;
            _scheduleRepository = scheduleRepository;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<ResponseBase> AddScheduleAsync(AddScheduleDto dto)
        {
            var response = new ResponseBase();

            dto.RemoveSeconds();

            var doctor = await _doctorRepository.GetByIdAsync(dto.DoctorId);
            if (doctor is null)
            {
                response.AddError("Médico não encontrado");
                return response;
            }

            var concurrentSchedules = await _scheduleRepository.GetConcurrentSchedulesAsync(dto.DoctorId, dto.StartAvailabilityDate, dto.EndAvailabilityDate);
            if (concurrentSchedules.Any())
            {
                response.AddError("Existem agendas criadas nesse período");
                return response;
            }

            var schedule = new Schedule
            {
                DoctorId = dto.DoctorId,
                StartAvailabilityDate = dto.StartAvailabilityDate,
                EndAvailabilityDate = dto.EndAvailabilityDate
            };

            await _scheduleRepository.AddAsync(schedule);

            response.AddData("Agenda adicionada com sucesso!");
            return response;
        }

        public async Task<ResponseBase> UpdateScheduleAsync(UpdateScheduleDto dto)
        {
            var response = new ResponseBase();

            dto.RemoveSeconds();

            var schedule = await _scheduleRepository.GetScheduleByIdAndDoctorIdAsync(dto.ScheduleId, dto.DoctorId);
            if (schedule is null)
            {
                response.AddError("Agenda não encontrada");
                return response;
            }

            if(schedule.StartAvailabilityDate.Date != dto.StartAvailabilityDate.Date)
            {
                response.AddError("Não é possível alterar uma agenda para outro dia. Remova esta e crie uma nova agenda");
                return response;
            }

            var concurrentSchedules = await _scheduleRepository.GetConcurrentSchedulesAsync(dto.DoctorId, dto.StartAvailabilityDate, dto.EndAvailabilityDate);
            if (concurrentSchedules.Any(x => x.Id != schedule.Id))
            {
                response.AddError("Existem agendas criadas nesse período");
                return response;
            }

            //Delete affected appointments
            var appointments = await _appointmentRepository.GetByDoctorIdAndInterval(dto.DoctorId, schedule.StartAvailabilityDate.Date, schedule.StartAvailabilityDate.Date.AddDays(1));
            foreach (var appointment in appointments)
            {
                if(appointment.StartDate >= schedule.EndAvailabilityDate || appointment.StartDate < schedule.StartAvailabilityDate)
                {
                    await _appointmentRepository.RemoveAsync(appointment);

                    //TODO: Send email
                }
            }

            schedule.StartAvailabilityDate = dto.StartAvailabilityDate;
            schedule.EndAvailabilityDate = dto.EndAvailabilityDate;

            await _scheduleRepository.UpdateAsync(schedule);

            response.AddData("Agenda alterada com sucesso!");
            return response;
        }

        public async Task<ResponseBase> DeleteScheduleAsync(Guid doctorId, Guid scheduleId)
        {
            var response = new ResponseBase();

            var schedule = await _scheduleRepository.GetScheduleByIdAndDoctorIdAsync(doctorId, scheduleId);
            if (schedule is null)
            {
                response.AddError("Agenda não encontrada");
                return response;
            }

            //Delete affected appointments
            var appointments = await _appointmentRepository.GetByDoctorIdAndInterval(doctorId, schedule.StartAvailabilityDate.Date, schedule.EndAvailabilityDate);
            foreach (var appointment in appointments)
            {
                if (appointment.StartDate >= schedule.EndAvailabilityDate || appointment.StartDate < schedule.StartAvailabilityDate)
                {
                    await _appointmentRepository.RemoveAsync(appointment);

                    //TODO: Send email
                }
            }

            await _scheduleRepository.RemoveAsync(schedule);

            response.AddData("Agenda deletada com sucesso!");
            return response;
        }

    }
}
