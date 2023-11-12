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
        public async Task<IActionResult> GenerateEventsSummary()
        {
            var document = await _filesService.GenerateEventsSummary();

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
        public IActionResult GenerateFestivalTable(int FestivalYear)
        {
            var document = _filesService.GenerateFestivalTable(FestivalYear);

            // Set the Content-Disposition header to force download with a specific file name
            var contentDisposition = new ContentDisposition
            {
                FileName = $"FestivalTable_{FestivalYear}.pdf",
                Inline = false,  // Set to false to force download
            };

            Response.Headers.Add("Content-Disposition", contentDisposition.ToString());



            return File(document, "application/pdf");


        }


    }
}
