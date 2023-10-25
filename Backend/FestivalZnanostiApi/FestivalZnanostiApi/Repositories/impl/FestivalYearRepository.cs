using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.DTOs.Extensions;
using FestivalZnanostiApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FestivalZnanostiApi.Repositories.impl
{
    public class FestivalYearRepository : IFestivalYearRepository
    {

        private readonly FestivalZnanostiContext _context;

        public FestivalYearRepository(FestivalZnanostiContext context)
        {
            _context = context;
        }

        public async Task<int> CreateFestivalYear(FestivalYearDto festivalYear)
        {

            FestivalYear newFestivalYear = new FestivalYear
            {
                Year = festivalYear.Year,
                Active = festivalYear.Active,
                Title = festivalYear.Title,
                Topic = festivalYear.Topic,
                Description = festivalYear.Description,
                StartDate = festivalYear.StartDate.Date,
                EndDate = festivalYear.EndDate.Date.AddDays(1),  //istestirao i to je to
                EditUntil = festivalYear.StartDate.Date.AddDays(-21), // submitter može editirat svoj event sve do 3 tjedna prije početka festivala (ili i uvest mogućnost da admin odabere do kada), admin može editirati sve evente čitavo vrijeme
            };

            _context.Add(newFestivalYear);
            await _context.SaveChangesAsync();

            return await Task.FromResult(newFestivalYear.Id);
        }

        public FestivalYearDto FindActiveFestivalYear()
        {
            FestivalYear? festival = _context.FestivalYear.Where(fy => fy.Active == 1).FirstOrDefault();
            if (festival is null)
            {
                throw new Exception($"There is no active FestivalYear");
            }
            return festival.AsFestivalYearDto();
        }

        public async Task<FestivalYear> FindById(int id)
        {
            var festivalYear = await _context.FestivalYear.FindAsync(id);
            if (festivalYear is null)
            {
                throw new Exception($"FestivalYear with id = {id} does not exists");
            }
            else
            {
                return festivalYear;
            }

        }

        public async Task<IEnumerable<FestivalYear>> GetFestivalYear()
        {
            return await _context.FestivalYear.ToListAsync();
        }


    }
}
