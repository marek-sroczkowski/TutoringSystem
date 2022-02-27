using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AvailabilityDtos;
using TutoringSystem.Application.Dtos.IntervalDtos;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Application.Parameters;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Services
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly IAvailabilityRepository availabilityRepository;
        private readonly IMapper mapper;

        public AvailabilityService(IAvailabilityRepository availabilityRepository, IMapper mapper)
        {
            this.availabilityRepository = availabilityRepository;
            this.mapper = mapper;
        }

        public async Task<AvailabilityDto> AddAvailabilityAsync(long tutorId, NewAvailabilityDto newAvailability)
        {
            if (!await ValidateNewAvailabilityAsync(newAvailability, tutorId))
            {
                return null;
            }

            var availability = mapper.Map<Availability>(newAvailability);
            availability.TutorId = tutorId;
            var created = await availabilityRepository.AddAvailabilityAsync(availability);

            return created ? mapper.Map<AvailabilityDto>(availability) : null;
        }

        public async Task<bool> RemoveAvailabilityAsync(long availabilityId)
        {
            var availability = await availabilityRepository.GetAvailabilityAsync(a => a.Id.Equals(availabilityId));

            return await availabilityRepository.RemoveAvailabilityAsync(availability);
        }

        public async Task<PagedList<AvailabilityDto>> GetAvailabilitiesByTutorAsync(long tutorId, AvailabilityParameters parameters)
        {
            Expression<Func<Availability, bool>> expression = a => a.TutorId.Equals(tutorId);
            FilterByStartDate(ref expression, parameters.StartDate);
            FilterByEndDate(ref expression, parameters.EndDate);
            var availabilities = await availabilityRepository.GetAvailabilitiesCollectionAsync(expression);
            var availabilityDtos = mapper.Map<ICollection<AvailabilityDto>>(availabilities);

            return PagedList<AvailabilityDto>.ToPagedList(availabilityDtos, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<PagedList<AvailabilityDto>> GetFutureAvailabilitiesByTutorAsync(long tutorId, FutureAvailabilityParameters parameters)
        {
            Expression<Func<Availability, bool>> expression = a => a.TutorId.Equals(tutorId) && a.Date >= DateTime.Now;
            FilterByEndDate(ref expression, parameters.EndDate);
            var availabilities = await availabilityRepository.GetAvailabilitiesCollectionAsync(expression);
            var availabilityDtos = mapper.Map<ICollection<AvailabilityDto>>(availabilities);

            return PagedList<AvailabilityDto>.ToPagedList(availabilityDtos, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<AvailabilityDetailsDto> GetAvailabilityByIdAsync(long availabilityId)
        {
            var availability = await availabilityRepository.GetAvailabilityAsync(a => a.Id.Equals(availabilityId), true);

            return mapper.Map<AvailabilityDetailsDto>(availability);
        }

        public async Task<AvailabilityDetailsDto> GetTodaysAvailabilityByTutorAsync(long tutorId)
        {
            var availability = await availabilityRepository.GetAvailabilityAsync(a => a.TutorId.Equals(tutorId) && a.Date.Date.Equals(DateTime.Now.Date), true);

            return mapper.Map<AvailabilityDetailsDto>(availability);
        }

        public async Task<bool> UpdateAvailabilityAsync(UpdatedAvailabilityDto updatedAvailability)
        {
            var existingAvailability = await availabilityRepository.GetAvailabilityAsync(a => a.Id.Equals(updatedAvailability.Id));
            if (!ValidateUpdatedAvailability(updatedAvailability, existingAvailability.Date))
            {
                return false;
            }

            var availability = mapper.Map(updatedAvailability, existingAvailability);

            return await availabilityRepository.UpdateAvailabilityAsync(availability);
        }

        private static void FilterByStartDate(ref Expression<Func<Availability, bool>> expression, DateTime? startDate)
        {
            if (!startDate.HasValue)
            {
                return;
            }

            ExpressionMerger.MergeExpression(ref expression, a => a.Date >= startDate.Value);
        }

        private static void FilterByEndDate(ref Expression<Func<Availability, bool>> expression, DateTime? endDate)
        {
            if (!endDate.HasValue)
            {
                return;
            }

            ExpressionMerger.MergeExpression(ref expression, a => a.Date <= endDate.Value);
        }

        private async Task<bool> ValidateNewAvailabilityAsync(NewAvailabilityDto newAvailability, long tutorId)
        {
            if (newAvailability.Date.Date < DateTime.Now.Date)
                return false;

            if (newAvailability.Intervals is null)
                return false;

            if (newAvailability.Intervals.Count > 40 || newAvailability.Intervals.Count == 0)
                return false;

            if ((await availabilityRepository.GetAvailabilityAsync(a => a.TutorId.Equals(tutorId) && a.Date.Date.Equals(newAvailability.Date.Date))) != null)
                return false;

            foreach (var interval in newAvailability.Intervals)
            {
                if (interval.StartTime >= interval.EndTime)
                    return false;

                if (interval.StartTime.Date != newAvailability.Date.Date)
                    return false;

                if (interval.EndTime.Date != newAvailability.Date.Date)
                    return false;

                if (interval.StartTime.AddMinutes(30) > interval.EndTime)
                    return false;

                if (CheckForDuplicatesInNew(interval, newAvailability.Intervals))
                    return false;
            }

            return true;
        }

        private bool ValidateUpdatedAvailability(UpdatedAvailabilityDto updatedAvailability, DateTime availabilityDate)
        {
            if (updatedAvailability.Intervals is null)
                return false;

            if (updatedAvailability.Intervals.Count > 40 || updatedAvailability.Intervals.Count == 0)
                return false;

            foreach (var interval in updatedAvailability.Intervals)
            {
                if (interval.StartTime >= interval.EndTime)
                    return false;

                if (interval.StartTime.Date != availabilityDate.Date)
                    return false;

                if (interval.EndTime.Date != availabilityDate.Date)
                    return false;

                if (interval.StartTime.AddMinutes(30) > interval.EndTime)
                    return false;

                if (CheckForDuplicatesInUpdated(interval, updatedAvailability.Intervals))
                    return false;

            }

            return true;
        }

        private static bool CheckForDuplicatesInNew(NewIntervalDto interval, ICollection<NewIntervalDto> intervals)
        {
            bool oneDuplicate = false;

            foreach (var i in intervals)
            {
                if (i.StartTime == interval.StartTime && i.EndTime == interval.EndTime && !oneDuplicate)
                {
                    oneDuplicate = true;
                    continue;
                }

                if (i.StartTime <= interval.EndTime && i.EndTime >= interval.StartTime)
                    return true;
            }

            return false;
        }

        private static bool CheckForDuplicatesInUpdated(UpdatedIntervalDto interval, ICollection<UpdatedIntervalDto> intervals)
        {
            bool oneDuplicate = false;

            foreach (var i in intervals)
            {
                if (i.StartTime== interval.StartTime && i.EndTime == interval.EndTime && !oneDuplicate)
                {
                    oneDuplicate = true;
                    continue;
                }

                if (i.StartTime <= interval.EndTime && i.EndTime >= interval.StartTime)
                    return true;
            }

            return false;
        }
    }
}
