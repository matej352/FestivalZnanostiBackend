namespace FestivalZnanostiApi.DTOs
{
    public class UpdateFestivalYearDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Topic { get; set; }
        public string? Description { get; set; }
        public DateTime? ApplicationStart { get; set; }     // should be before StartDate and EndDate, should be before EditUntil
        public DateTime? EditUntil { get; set; }    // should be before StartDate and EndDate, should be after ApplicationStart
    }
}
