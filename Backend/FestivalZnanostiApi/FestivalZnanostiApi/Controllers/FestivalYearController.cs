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

        [HttpGet]
        [Route("FestivalYears")]
        public Task<IEnumerable<FestivalYearDto>> GetFestivalYears()
        {
            var festivalYears = _festivalYearService.GetFestivalYears();
            return festivalYears;
        }


        [HttpGet]
        [Route("FestivalYear")]
        public Task<FestivalYearDto> GetFestivalYear(int festivalYearId)
        {
            var festivalYear = _festivalYearService.GetFestivalYear(festivalYearId);
            return festivalYear;
        }



        [HttpPost]
        [Route("Create")]
        public Task<FestivalYearDto> CreateFestivalYear(FestivalYearDto FestivalYear)
        {
            var newFestivalYear = _festivalYearService.CreateFestivalYear(FestivalYear);
            return newFestivalYear;
        }
    }
}