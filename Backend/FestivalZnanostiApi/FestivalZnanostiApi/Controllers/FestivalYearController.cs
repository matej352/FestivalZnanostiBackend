using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.DTOs.Validators;
using FestivalZnanostiApi.Enums;
using FestivalZnanostiApi.Middlewares.UserContext;
using FestivalZnanostiApi.Models;
using FestivalZnanostiApi.Servicess;
using FestivalZnanostiApi.Servicess.impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FestivalZnanostiApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FestivalYearController : ControllerBase
    {

        private readonly IFestivalYearService _festivalYearService;
        private readonly ILogger<FestivalYearController> _logger;
        private readonly CreateFestivalYearDtoValidator _validations;

        public FestivalYearController(ILogger<FestivalYearController> logger, IFestivalYearService festivalYearService, CreateFestivalYearDtoValidator validations)
        {
            _logger = logger;
            _festivalYearService = festivalYearService;
            _validations = validations;

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


        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<FestivalYearDto>> CreateFestivalYear(CreateFestivalYearDto FestivalYear)
        {
            var validationResult = await _validations.ValidateAsync(FestivalYear);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var newFestivalYear = await _festivalYearService.CreateFestivalYear(FestivalYear);
            return Ok(newFestivalYear);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut]
        [Route("UpdateFestivalYear/{id}")]
        public async Task<ActionResult<FestivalYearDto>> UpdateFestivalYear(int id, UpdateFestivalYearDto updateFestivalYearDto)
        {
            if (id != updateFestivalYearDto.Id)
            {
                return BadRequest("Festival year id mismatch!");
            }

            var updatedFestivalYear = await _festivalYearService.UpdateFestivalYear(updateFestivalYearDto);

            return Ok(updatedFestivalYear);


        }
    }
}