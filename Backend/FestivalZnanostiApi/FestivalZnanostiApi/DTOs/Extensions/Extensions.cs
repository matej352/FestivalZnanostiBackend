using FestivalZnanostiApi.Enums;
using FestivalZnanostiApi.Models;

namespace FestivalZnanostiApi.DTOs.Extensions
{
    public static class Extensions
    {

        public static FestivalYearDto AsFestivalYearDto(this FestivalYear fy)
        {
            return new FestivalYearDto
            {
                Id = fy.Id,
                Year = fy.Year,
                Active = fy.Active,
                Title = fy.Title,
                Topic = fy.Topic,
                Description = fy.Description,
                StartDate = fy.StartDate,
                EndDate = fy.EndDate,
                EditUntil = fy.EditUntil
            };
        }



        public static EventDto AsEventDto(this Event e)
        {
            return new EventDto
            {
                Id = e.Id,
                Title = e.Title,
                Status = (EventStatus)e.Status,
                Type = e.Type,
                VisitorsCount = e.VisitorsCount,
                Equipment = e.Equipment,
                Summary = e.Summary,
                SubmitterEmail = e.Submitter.Email,
                Location = new LocationDto()
                {
                    Id = e.LocationId,
                    Name = e.Location.Name,
                    ParentLocationId = e.Location.ParentLocationId,
                    ParentLocationName = e.Location.ParentLocation?.Name,
                },
                Lecturers = e.Lecturer.Select(l => l.AsLecturerDto()).ToList(),
                ParticipantsAges = e.ParticipantsAge.Select(pa => pa.AsParticipantsAgeDto()).ToList(),
                TimeSlots = e.TimeSlot.Select(ts => ts.AsTimeSlotDto()).ToList()
            };
        }



        public static LecturerDto AsLecturerDto(this Lecturer l)
        {
            return new LecturerDto
            {
                FirstName = l.FirstName,
                LastName = l.LastName,
                Phone = l.Phone,
                Email = l.Email,
                Type = (LecturerType)Convert.ToInt32(l.Type),
                Resume = l.Resume
            };
        }


        public static ParticipantsAgeDto AsParticipantsAgeDto(this ParticipantsAge pa)
        {
            return new ParticipantsAgeDto
            {
                Id = pa.Id,
                Age = pa.Age,
                Label = pa.Label,
            };
        }


        public static TimeSlotDto AsTimeSlotDto(this TimeSlot ts)
        {
            return new TimeSlotDto
            {
                Id = ts.Id,
                Start = ts.Start,
            };
        }


        public static AccountDto AsAccountDto(this Account a)
        {
            return new AccountDto
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Email = a.Email,
                Role = a.Role,
            };
        }



        public static LocationDto AsLocationDto(this Location l)
        {
            return new LocationDto
            {
                Id = l.Id,
                Name = l.Name,
                ParentLocationId = l.ParentLocationId,
                ParentLocationName = l.ParentLocation?.Name
            };
        }



    }
}
