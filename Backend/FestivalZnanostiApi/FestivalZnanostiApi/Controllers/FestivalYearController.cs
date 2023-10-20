using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.Models;
using FestivalZnanostiApi.Servicess;
using FestivalZnanostiApi.Servicess.impl;
using Microsoft.AspNetCore.Mvc;

namespace FestivalZnanostiApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FestivalYearController : ControllerBase
    {

        private readonly IFestivalYearService _festivalYearService;
        private readonly ILogger<FestivalYearController> _logger;

        public FestivalYearController(ILogger<FestivalYearController> logger, IFestivalYearService festivalYearService)
        {
            _logger = logger;
            _festivalYearService = festivalYearService;

        }

        [HttpGet(Name = "festival")]
        public Task<IEnumerable<FestivalYear>> Get()
        {
            var festivalYear = _festivalYearService.Get();
            return festivalYear;
        }



        [HttpPost]
        public Task<FestivalYearDto> CreateFestivalYear(FestivalYearDto festivalYear)
        {
            var newFestivalYear = _festivalYearService.CreateFestivalYear(festivalYear);
            return newFestivalYear;
        }
    }
}