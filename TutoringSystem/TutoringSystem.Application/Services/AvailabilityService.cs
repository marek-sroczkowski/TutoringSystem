﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AvailabilityDtos;
using TutoringSystem.Application.Dtos.IntervalDtos;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Helpers;
using TutoringSystem.Domain.Parameters;
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
            if (!(await ValidateNewAvailabilityAsync(newAvailability, tutorId)))
                return null;

            var availability = mapper.Map<Availability>(newAvailability);
            availability.TutorId = tutorId;
            var created = await availabilityRepository.AddAvailabilityAsync(availability);
            if (!created)
                return null;

            return mapper.Map<AvailabilityDto>(availability);
        }

        public async Task<bool> DeleteAvailabilityAsync(long availabilityId)
        {
            var availability = await availabilityRepository.GetAvailabilityByIdAsync(availabilityId);

            return await availabilityRepository.DeleteAvailabilityAsync(availability);
        }

        public async Task<PagedList<AvailabilityDto>> GetAvailabilitiesByTutorAsync(long tutorId, AvailabilityParameters parameters)
        {
            var availabilities = await availabilityRepository.GetAvailabilitiesByTutorIdAsync(tutorId);
            FilterByStartDate(ref availabilities, parameters.StartDate);
            FilterByEndDate(ref availabilities, parameters.EndDate);
            var availabilityDtos = mapper.Map<ICollection<AvailabilityDto>>(availabilities);

            return PagedList<AvailabilityDto>.ToPagedList(availabilityDtos, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<AvailabilityDetailsDto> GetAvailabilityByIdAsync(long availabilityId)
        {
            var availability = await availabilityRepository.GetAvailabilityByIdAsync(availabilityId);

            return mapper.Map<AvailabilityDetailsDto>(availability);
        }

        public async Task<PagedList<AvailabilityDto>> GetFutureAvailabilitiesByTutorAsync(long tutorId, FutureAvailabilityParameters parameters)
        {
            var availabilities = await availabilityRepository.GetFutureAvailabilitiesByTutorIdAsync(tutorId);
            FilterByEndDate(ref availabilities, parameters.EndDate);
            var availabilityDtos = mapper.Map<ICollection<AvailabilityDto>>(availabilities);

            return PagedList<AvailabilityDto>.ToPagedList(availabilityDtos, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<AvailabilityDetailsDto> GetTodaysAvailabilityByTutorAsync(long tutorId)
        {
            var availability = await availabilityRepository.GetTodaysAvailabilityByTutorIdAsync(tutorId);

            return mapper.Map<AvailabilityDetailsDto>(availability);
        }

        public async Task<bool> UpdateAvailabilityAsync(UpdatedAvailabilityDto updatedAvailability)
        {
            var existingAvailability = await availabilityRepository.GetAvailabilityByIdAsync(updatedAvailability.Id);
            if (!ValidateUpdatedAvailability(updatedAvailability, existingAvailability.Date))
                return false;

            var availability = mapper.Map(updatedAvailability, existingAvailability);

            return await availabilityRepository.UpdateAvailabilityAsync(availability);
        }

        private void FilterByStartDate(ref IEnumerable<Availability> availabilities, DateTime? startDate)
        {
            if (!startDate.HasValue)
                return;

            availabilities = availabilities.Where(r => r.Date >= startDate.Value);
        }

        private void FilterByEndDate(ref IEnumerable<Availability> availabilities, DateTime? endDate)
        {
            if (!endDate.HasValue)
                return;

            availabilities = availabilities.Where(r => r.Date <= endDate.Value);
        }

        private async Task<bool> ValidateNewAvailabilityAsync(NewAvailabilityDto newAvailability, long tutorId)
        {
            if (newAvailability.Date.Date < DateTime.Now.Date)
                return false;

            if (newAvailability.Intervals is null)
                return false;

            if (newAvailability.Intervals.Count > 40 || newAvailability.Intervals.Count == 0)
                return false;

            if ((await availabilityRepository.GetAvailabilityByTutorIdAndDateAsync(tutorId, newAvailability.Date)) != null)
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

        private bool CheckForDuplicatesInNew(NewIntervalDto interval, ICollection<NewIntervalDto> intervals)
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

        private bool CheckForDuplicatesInUpdated(UpdatedIntervalDto interval, ICollection<UpdatedIntervalDto> intervals)
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