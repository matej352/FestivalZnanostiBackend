using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.Enums;
using FestivalZnanostiApi.Models;
using FestivalZnanostiApi.Repositories;
using FestivalZnanostiApi.Repositories.impl;
using FestivalZnanostiApi.Servicess;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FestivalZnanostiApi.Services.impl
{
    public class TimeSlotService : ITimeSlotService
    {

        private readonly ITimeSlotRepository _repo;
        private readonly ILocationRepository _locationRepository;

        private readonly IFestivalYearRepository _festivalYearRepository;

        public const int WINDOW_START = 10;
        public const int WINDOW_END = 20;


        public TimeSlotService(ITimeSlotRepository timeSlotRepository, IFestivalYearRepository festivalYearRepository, ILocationRepository locationRepository)
        {
            _repo = timeSlotRepository;
            _festivalYearRepository = festivalYearRepository;
            _locationRepository = locationRepository;
        }

        public async Task CreateTimeSlots(DateTime startDate, DateTime endDate, int festivalYearId)
        {

            List<TimeSlotTemp> timeSlots = new List<TimeSlotTemp>();

            var locationsWithTrackedTimeslots = await _locationRepository.GetLocationsWithTrackedTimeslots();

            foreach (var locationTemp in locationsWithTrackedTimeslots)
            {
                timeSlots.AddRange(GenerateTimeSlots(startDate, endDate, locationTemp, festivalYearId));    //create timeslots for locations on which we track parallel events count (locations with TimeSlotTracked = 1 --> Locations in Tehnički muzej)
            }

            timeSlots.AddRange(GenerateNonTrackableTimeSlots(startDate, endDate, festivalYearId));    //create timeslots for locations on which we DO NOT track parallel events count (locations with TimeSlotTracked = 0 --> Locations outside of Tehnički muzej)


            await _repo.SaveTimeSlots(timeSlots);

        }











        public List<TimeSlotTemp> GenerateTimeSlots(DateTime startDate, DateTime endDate, Location location, int festivalYearId)   // for example: 20/21/2023 at midnight (12am)
        {
            var timeSlots = new List<TimeSlotTemp>();

            // Loop through each day from StartDate to EndDate
            for (var currentDate = startDate.Date; currentDate <= endDate.Date; currentDate = currentDate.AddDays(1))
            {
                // Create time slots with 30-minute duration from 8 am to 8 pm
                var startTime = currentDate.AddHours(WINDOW_START);
                var endTime = currentDate.AddHours(WINDOW_END).AddMinutes(-(int)location.EventDuration); // Adjust to end at 7:30/7:15 pm

                while (startTime <= endTime)
                {
                    var slot = new TimeSlotTemp
                    {
                        StartTime = startTime,
                        EndTime = startTime.AddMinutes((int)location.EventDuration),
                        LocationId = location.Id,
                        FestivalYearId = festivalYearId
                    };

                    timeSlots.Add(slot);
                    startTime = startTime.AddMinutes((int)location.EventDuration); // Move to the next 30-minute/45-minute slot
                }
            }

            return timeSlots;
        }


        public List<TimeSlotTemp> GenerateNonTrackableTimeSlots(DateTime startDate, DateTime endDate, int festivalYearId)   // for example: 20/21/2023 at midnight (12am)
        {
            var timeSlots = new List<TimeSlotTemp>();

            // Loop through each day from StartDate to EndDate
            for (var currentDate = startDate.Date; currentDate <= endDate.Date; currentDate = currentDate.AddDays(1))
            {
                // Create time slots with 30-minute duration from 8 am to 8 pm
                var startTime = currentDate.AddHours(WINDOW_START);
                var endTime = currentDate.AddHours(WINDOW_END).AddMinutes(-60); // Adjust to end at 7:30/7:15 pm

                while (startTime <= endTime)
                {
                    var slot = new TimeSlotTemp
                    {
                        StartTime = startTime,
                        EndTime = startTime.AddMinutes(60),
                        FestivalYearId = festivalYearId
                    };

                    timeSlots.Add(slot);
                    startTime = startTime.AddMinutes(60); // Move to the next 30-minute/45-minute slot
                }
            }

            return timeSlots;
        }

        public async Task<IEnumerable<TimeSlotDto>> GetAvailableTimeSlots(int locationId, bool isIzlozba)
        {
            var festivalYear = await _festivalYearRepository.FindActiveFestivalYear();

            return await _repo.GetAvailableTimeSlots(locationId, festivalYear.Id, isIzlozba);
        }
    }


}

public class TimeSlotTemp
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int? LocationId { get; set; }
    public int FestivalYearId { get; set; }
}

public class LocationTemp
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int EventDuration { get; set; }
}
