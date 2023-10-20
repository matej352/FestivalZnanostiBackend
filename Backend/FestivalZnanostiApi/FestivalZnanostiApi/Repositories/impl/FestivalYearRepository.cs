using FestivalZnanostiApi.DTOs;
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

        public async Task<int> CreateFestivalYear(FestivalYear festivalYear)
        {
            _context.Add(festivalYear);
            await _context.SaveChangesAsync();

            //TODO: CREATE TIMESLOTS (30 MIN AND 45 MIN) FOR DATES BETWEEN STARTDATE AND ENDDATE AND BETWEEN 8H AND 20H LOCAL TIME

            return await Task.FromResult(festivalYear.Id);
        }

        public async Task<FestivalYear> FindById(int id)
        {
            var festivalYear = await _context.FestivalYear.FindAsync(id);
            if (festivalYear is null)
            {
                throw new Exception($"FestivalYear with id = {id} does not exists!");
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
