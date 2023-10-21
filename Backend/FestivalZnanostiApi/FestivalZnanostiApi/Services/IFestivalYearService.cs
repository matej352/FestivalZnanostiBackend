using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FestivalZnanostiApi.Servicess
{
    public interface IFestivalYearService
    {
        public Task<IEnumerable<FestivalYear>> Get();
        public Task<FestivalYearDto> CreateFestivalYear(FestivalYearDto festivalYear);
        public FestivalYearDto GetActiveFestivalYear();
    }
}
