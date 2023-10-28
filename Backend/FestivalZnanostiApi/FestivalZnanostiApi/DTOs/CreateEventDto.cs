using FestivalZnanostiApi.Enums;
using FestivalZnanostiApi.Models;

namespace FestivalZnanostiApi.DTOs
{
    public class CreateEventDto
    {
        public string Title { get; set; }

        public EventType Type { get; set; }

        public int VisitorsCount { get; set; }

        public string Equipment { get; set; }

        public string Summary { get; set; }

        public SubmitterDto Submitter { get; set; }

        public int LocationId { get; set; }

        public List<LecturerDto> Lecturers { get; set; }

        public List<ParticipantsAgeDto> ParticipantsAges { get; set; }

        public List<TimeSlotDto> TimeSlots { get; set; }
    }
}
