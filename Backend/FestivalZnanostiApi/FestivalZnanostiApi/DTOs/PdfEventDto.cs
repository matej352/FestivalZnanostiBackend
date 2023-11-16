using FestivalZnanostiApi.Enums;
using FestivalZnanostiApi.Models;

namespace FestivalZnanostiApi.DTOs
{
    public class PdfEventDto
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Type { get; set; }

        public int VisitorsCount { get; set; }

        public string Summary { get; set; }

        public Location Location { get; set; }

        public List<LecturerDto> Lecturers { get; set; }

        public List<ParticipantsAgeDto> ParticipantsAges { get; set; }

        public List<TimeSlotDto> TimeSlots { get; set; }
    }
}
