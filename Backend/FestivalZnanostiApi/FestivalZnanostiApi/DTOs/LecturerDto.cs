using FestivalZnanostiApi.Enums;

namespace FestivalZnanostiApi.DTOs
{
    public class LecturerDto
    {
        public int? Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public LecturerType Type { get; set; }

        public string Resume { get; set; }
    }
}
