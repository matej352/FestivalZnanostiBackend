using FestivalZnanostiApi.Enums;

namespace FestivalZnanostiApi.DTOs
{
    public class UpdateEventDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public EventType Type { get; set; }

        public int VisitorsCount { get; set; }

        public string Equipment { get; set; }

        public string Summary { get; set; }

        public int LocationId { get; set; }

        public List<LecturerDto>? LecturersForUpdate { get; set; }

        public List<LecturerDto>? LecturersForCreate { get; set; }

        public List<int>? LecturersForDelete { get; set; }

        public List<ParticipantsAgeDto> ParticipantsAges { get; set; }

        public List<int> TimeSlotIds { get; set; }
    }
}
