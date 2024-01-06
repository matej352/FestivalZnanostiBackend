using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.Enums;
using FestivalZnanostiApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FestivalZnanostiApi.Servicess
{
    public interface IFestivalYearService
    {
        public Task<IEnumerable<FestivalYearDto>> GetFestivalYears();

        public Task<FestivalYearDto> GetFestivalYear(int festivalYearId);

        public Task<FestivalYearDto> CreateFestivalYear(CreateFestivalYearDto festivalYear);

        public Task<FestivalYearDto> GetActiveFestivalYear();

        public Task<FestivalYearDto> UpdateFestivalYear(UpdateFestivalYearDto updateFestivalYear);

        public Task ChangeFestivalYearActiveStatus(int id, FestivalYearActivityStatus active);


    }
}
