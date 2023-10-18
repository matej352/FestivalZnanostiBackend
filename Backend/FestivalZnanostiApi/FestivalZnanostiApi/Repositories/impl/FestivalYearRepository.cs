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


        public async Task<IEnumerable<FestivalYear>> GetFestivalYear()
        {
            return await _context.FestivalYear.ToListAsync();
        }


    }
}
