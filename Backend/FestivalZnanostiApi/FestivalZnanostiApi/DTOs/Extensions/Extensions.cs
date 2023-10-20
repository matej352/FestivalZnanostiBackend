using FestivalZnanostiApi.Models;

namespace FestivalZnanostiApi.DTOs.Extensions
{
    public static class Extensions
    {

        public static FestivalYearDto AsFestivalYearDto(this FestivalYear fy)
        {
            return new FestivalYearDto
            {
                Year = fy.Year,
                Active = fy.Active,
                Title = fy.Title,
                Topic = fy.Topic,
                Description = fy.Description,
                StartDate = fy.StartDate,
                EndDate = fy.EndDate
            };
        }


    }
}
