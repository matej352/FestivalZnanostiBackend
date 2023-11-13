using FestivalZnanostiApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PdfSharpCore.Pdf;
using System.Net.Mime;

namespace FestivalZnanostiApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilesController : ControllerBase
    {

        private readonly IFilesService _filesService;

        public FilesController(IFilesService filesService)
        {
            _filesService = filesService;
        }




        //[Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("EventSummary")]
        public async Task<IActionResult> GenerateEventsSummary(int? festivalYearId = null)
        {
            var document = await _filesService.GenerateEventsSummary(festivalYearId);

            // Set the Content-Disposition header to force download with a specific file name
            var contentDisposition = new ContentDisposition
            {
                FileName = $"EventSummaries.pdf",
                Inline = false,  // Set to false to force download
            };

            Response.Headers.Add("Content-Disposition", contentDisposition.ToString());



            return File(document, "application/pdf");


        }

        //[Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("FestivalTable")]
        public async Task<IActionResult> GenerateFestivalTable(int? festivalYear = null)
        {
            var document = await _filesService.GenerateFestivalTable(festivalYear);

            // Set the Content-Disposition header to force download with a specific file name
            var contentDisposition = new ContentDisposition
            {
                FileName = $"FestivalTable.pdf",
                Inline = false,  // Set to false to force download
            };

            Response.Headers.Add("Content-Disposition", contentDisposition.ToString());



            return File(document, "application/pdf");


        }


    }
}
