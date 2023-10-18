using FestivalZnanostiApi.Models;
using FestivalZnanostiApi.Repositories;
using FestivalZnanostiApi.Repositories.impl;

namespace FestivalZnanostiApi.Servicess.impl
{
    public class FestivalYearService : IFestivalYearService
    {

        private readonly IFestivalYearRepository _repo;

        public FestivalYearService(IFestivalYearRepository repo) 
        {
            _repo = repo;
        }

        public Task<IEnumerable<FestivalYear>> Get()
        {
            return _repo.GetFestivalYear();
        }
    }
}
