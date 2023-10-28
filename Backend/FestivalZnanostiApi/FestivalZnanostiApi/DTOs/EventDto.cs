using FestivalZnanostiApi.Enums;

namespace FestivalZnanostiApi.DTOs
{
    public class EventDto
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public EventStatus? Status { get; set; }    // Only admin can see status of an event

        public string Type { get; set; }

        public int VisitorsCount { get; set; }

        public string Equipment { get; set; }

        public string Summary { get; set; }

        public string? SubmitterEmail { get; set; }  // Only admin can see email of event submitter

        public string Location { get; set; }

        public List<LecturerDto> Lecturers { get; set; }

        public List<ParticipantsAgeDto> ParticipantsAges { get; set; }

        public List<TimeSlotDto> TimeSlots { get; set; }
    }
}
