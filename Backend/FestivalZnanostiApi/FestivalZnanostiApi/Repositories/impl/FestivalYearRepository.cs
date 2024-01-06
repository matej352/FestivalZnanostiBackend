using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.DTOs.Extensions;
using FestivalZnanostiApi.Enums;
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
                EditUntil = festivalYear.EditUntil ?? festivalYear.StartDate.Date.AddDays(-21), // submitter može editirat svoj event sve do 3 tjedna prije početka festivala (ili i uvest mogućnost da admin odabere do kada), admin može editirati sve evente čitavo vrijeme
                ApplicationStart = festivalYear.ApplicationStart,
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

        public async Task<FestivalYearDto> FindActiveFestivalYear()
        {
            FestivalYear? festival = await _context.FestivalYear.Where(fy => fy.Active == 1).FirstOrDefaultAsync();
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

        public async Task<int> UpdateFestivalYear(UpdateFestivalYearDto updateFestivalYear)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {


                    // Retrieve the existing festival year
                    var existingFestivalYear = await _context.FestivalYear
                        .FirstOrDefaultAsync(fy => fy.Id == updateFestivalYear.Id);

                    if (existingFestivalYear == null)
                    {
                        throw new Exception($"Festival year with ID {updateFestivalYear.Id} not found.");
                    }

                    // Dates Validation 
                    if (updateFestivalYear.ApplicationStart > updateFestivalYear.EditUntil || updateFestivalYear.ApplicationStart > existingFestivalYear.EditUntil ||
                        updateFestivalYear.EditUntil < updateFestivalYear.ApplicationStart || updateFestivalYear.EditUntil < existingFestivalYear.ApplicationStart)
                    {
                        throw new Exception($"ApplicationStart should be before EditUntil.");
                    }



                    if (updateFestivalYear.ApplicationStart > existingFestivalYear.StartDate || updateFestivalYear.ApplicationStart > existingFestivalYear.EndDate ||
                        updateFestivalYear.EditUntil > existingFestivalYear.StartDate || updateFestivalYear.EditUntil > existingFestivalYear.EndDate)
                    {
                        throw new Exception($"Both ApplicationStart and EditUntil should be before StartDate and EndDate");
                    }




                    // Update the existing festival year properties
                    existingFestivalYear.Title = updateFestivalYear.Title ?? existingFestivalYear.Title;
                    existingFestivalYear.Topic = updateFestivalYear.Topic ?? existingFestivalYear.Topic;
                    existingFestivalYear.Description = updateFestivalYear.Description ?? existingFestivalYear.Description;
                    existingFestivalYear.ApplicationStart = updateFestivalYear.ApplicationStart ?? existingFestivalYear.ApplicationStart;
                    existingFestivalYear.EditUntil = updateFestivalYear.EditUntil ?? existingFestivalYear.EditUntil;


                    await _context.SaveChangesAsync();

                    transaction.Commit();

                    return existingFestivalYear.Id;
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    throw new Exception($"Problem while updating FestivalYear with ID {updateFestivalYear.Id}! \n" + exception.Message);
                }
            }
        }

        public async Task ChangeFestivalYearActiveStatus(int id, FestivalYearActivityStatus active)
        {
            // Retrieve the existing festival year
            var festivalYear = await _context.FestivalYear
                .FirstOrDefaultAsync(fy => fy.Id == id);


            if (festivalYear == null)
            {
                throw new Exception($"Festival year with ID {id} not found.");
            }

            if (active == FestivalYearActivityStatus.Active)
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

            festivalYear.Active = (int)active;

            await _context.SaveChangesAsync();

        }
    }
}
