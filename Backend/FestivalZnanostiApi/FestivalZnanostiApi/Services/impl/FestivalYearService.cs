using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.DTOs.Extensions;
using FestivalZnanostiApi.Models;
using FestivalZnanostiApi.Repositories;
using FestivalZnanostiApi.Repositories.impl;
using Microsoft.AspNetCore.Mvc;

namespace FestivalZnanostiApi.Servicess.impl
{
    public class FestivalYearService : IFestivalYearService
    {

        private readonly IFestivalYearRepository _repo;

        public FestivalYearService(IFestivalYearRepository repo)
        {
            _repo = repo;
        }

        public Task<FestivalYearDto> CreateFestivalYear(FestivalYearDto festivalYear)
        {
            try
            {
                FestivalYear newFestivalYear = new FestivalYear
                {
                    Year = festivalYear.Year,
                    Active = festivalYear.Active,
                    Title = festivalYear.Title,
                    Topic = festivalYear.Topic,
                    Description = festivalYear.Description,
                    StartDate = festivalYear.StartDate,
                    EndDate = festivalYear.EndDate
                };

                var id = _repo.CreateFestivalYear(newFestivalYear).Result;


                FestivalYear createdFestivalYear = _repo.FindById(id).Result;
                return Task.FromResult(createdFestivalYear.AsFestivalYearDto());

            }
            catch (Exception ex)
            {
                throw new Exception($"An problem occurred while creating new FestivalYear.");
            }

        }

        public Task<IEnumerable<FestivalYear>> Get()
        {
            return _repo.GetFestivalYear();
        }
    }
}
