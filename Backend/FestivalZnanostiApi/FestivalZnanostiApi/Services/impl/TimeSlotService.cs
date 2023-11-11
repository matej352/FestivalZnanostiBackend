using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.Enums;
using FestivalZnanostiApi.Models;
using FestivalZnanostiApi.Repositories;
using FestivalZnanostiApi.Servicess;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FestivalZnanostiApi.Services.impl
{
    public class TimeSlotService : ITimeSlotService
    {

        private readonly ITimeSlotRepository _repo;

        private readonly IFestivalYearRepository _festivalYearRepository;

        public const int WINDOW_START = 8;
        public const int WINDOW_END = 11;


        // TODO: Ovo treba dohvatiti iz baze (dohvati lokacije koje TimeSlotsTracked = 1)
        private readonly List<LocationTemp> _LocationTempList = new List<LocationTemp>()
        {
            new LocationTemp
            {
                Id = 2,
                Name = "Kino dvorana",
                EventDuration = 45
            },
            new LocationTemp
            {
                Id = 3,
                Name = "Izložbena dvorana",
                EventDuration = 30
            },
             new LocationTemp
            {
                Id = 4,
                Name = "Dvorište",
                EventDuration = 30
            }
        };


        public TimeSlotService(ITimeSlotRepository timeSlotRepository, IFestivalYearRepository festivalYearRepository)
        {
            _repo = timeSlotRepository;
            _festivalYearRepository = festivalYearRepository;
        }

        public async Task CreateTimeSlots(DateTime startDate, DateTime endDate)
        {

            List<TimeSlotTemp> timeSlots = new List<TimeSlotTemp>();

            foreach (var locationTemp in _LocationTempList)
            {
                timeSlots.AddRange(GenerateTimeSlots(startDate, endDate, locationTemp));    //create timeslots for locations on which we track parallel events count (locations with TimeSlotTracked = 1 --> Locations in Tehnički muzej)
            }

            timeSlots.AddRange(GenerateNonTrackableTimeSlots(startDate, endDate));    //create timeslots for locations on which we DO NOT track parallel events count (locations with TimeSlotTracked = 0 --> Locations outside of Tehnički muzej)


            await _repo.SaveTimeSlots(timeSlots);

        }











        public List<TimeSlotTemp> GenerateTimeSlots(DateTime startDate, DateTime endDate, LocationTemp location)   // for example: 20/21/2023 at midnight (12am)
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
                        LocationId = location.Id
                    };

                    timeSlots.Add(slot);
                    startTime = startTime.AddMinutes((int)location.EventDuration); // Move to the next 30-minute/45-minute slot
                }
            }

            return timeSlots;
        }


        public List<TimeSlotTemp> GenerateNonTrackableTimeSlots(DateTime startDate, DateTime endDate)   // for example: 20/21/2023 at midnight (12am)
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
                        EndTime = startTime.AddMinutes(60)
                    };

                    timeSlots.Add(slot);
                    startTime = startTime.AddMinutes(60); // Move to the next 30-minute/45-minute slot
                }
            }

            return timeSlots;
        }

        public Task<IEnumerable<TimeSlotDto>> GetAvailableTimeSlots(int locationId)
        {
            var festivalYear = _festivalYearRepository.FindActiveFestivalYear();

            return _repo.GetAvailableTimeSlots(locationId, festivalYear.StartDate, festivalYear.EndDate);
        }
    }


}

public class TimeSlotTemp
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int? LocationId { get; set; }
}

public class LocationTemp
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int EventDuration { get; set; }
}
