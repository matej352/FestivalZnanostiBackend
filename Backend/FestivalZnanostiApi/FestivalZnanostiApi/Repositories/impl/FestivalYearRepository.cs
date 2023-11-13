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

        public async Task<int> CreateFestivalYear(CreateFestivalYearDto festivalYear)
        {

            var correspondingFestivalYear = await _context.FestivalYear.Where(fy => fy.Year == festivalYear.Year).FirstOrDefaultAsync();

            if (correspondingFestivalYear != null)
            {
                throw new Exception($"Festival year for year {correspondingFestivalYear.Year} already exists!");
            }

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

            if (festivalYear.Active == 1)
            {
                // Set Active to 0 for every other festival year because at a time only 1 festival year can be active
                var activeFestivalYears = await _context.FestivalYear
                    .Where(fy => fy.Active == 1)
                    .ToListAsync();

                foreach (var activeFestivalYear in activeFestivalYears)
                {
                    activeFestivalYear.Active = 0;
                }
            }

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

        public async Task<IEnumerable<FestivalYearDto>> GetFestivalYears()
        {
            var festivalYearList = await _context.FestivalYear.ToListAsync();
            List<FestivalYearDto> festivalYearDtoList = festivalYearList.Select(fy => fy.AsFestivalYearDto()).ToList();
            return festivalYearDtoList;
        }


        public async Task<FestivalYearDto> GetFestivalYear(int festivalYearId)
        {
            FestivalYear? festival = await _context.FestivalYear.FindAsync(festivalYearId);

            if (festival is null)
            {
                throw new Exception($"There is no FestivalYear with id = {festivalYearId}");
            }

            return festival.AsFestivalYearDto();
        }

    }
}
