namespace FestivalZnanostiApi.DTOs
{
    public class LocationDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParentLocationId { get; set; }

        public string? ParentLocationName { get; set; }
    }
}
