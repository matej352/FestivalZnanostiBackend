using DinkToPdf;
using DinkToPdf.Contracts;
using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.Models;
using FestivalZnanostiApi.Servicess;
using PdfSharpCore.Pdf;
using System.Text;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace FestivalZnanostiApi.Services.impl
{
    public class FilesService : IFilesService
    {

        private readonly IConverter _converter;
        private readonly IEventsService _eventsService;
        private readonly IFestivalYearService _festivalYearService;


        public FilesService(IConverter converter, IEventsService eventsService, IFestivalYearService festivalYearService)
        {
            _converter = converter;
            _eventsService = eventsService;
            _festivalYearService = festivalYearService;
        }


        public async Task<byte[]> GenerateEventsSummary(int? festivalYearId = null)
        {

            var blobalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 30, Bottom = 30, Left = 25, Right = 25 },
                //DocumentTitle = "PdfReport",

            };

            var events = await _eventsService.GetEvents(festivalYearId);

            string htmlContent = CreateEventsSummaryHtmlContent(events);

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlContent,
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "styles.css") },    //ovo ne radi
            };

            var pdf = new HtmlToPdfDocument
            {
                GlobalSettings = blobalSettings,
                Objects = { objectSettings },
            };

            var file = _converter.Convert(pdf);

            return file;
        }

        private string CreateEventsSummaryHtmlContent(IEnumerable<EventDto> events)
        {
            var sb = new StringBuilder();

            foreach (var eventDto in events)
            {
                sb.Append(@$"
                                <h3 style=""font-family: 'Arial'"">{eventDto.Title}</h3>
                                <h3 style=""font-family: 'Arial'; display: inline"">Datum:</h3>
                                <span style=""font-family: 'Arial'; margin-left:5px"">08. travanj 2019., 10.30 – 11.00</span>
                                <br>
                                <br>
                               
                               


                ");

                //TODO: datume


                var lokacija = "";
                if (eventDto.Location.ParentLocationId is not null)
                {
                    lokacija += eventDto.Location.ParentLocationName + " | ";
                }
                lokacija += eventDto.Location.Name;

                sb.Append(@$"
                                <h3 style=""font-family: 'Arial'; display: inline"">Lokacija: </h3>
                                <span style=""font-family: 'Arial'"">{lokacija}</span>
                                <br>
                                <br>
                               


                ");

                var publika = "";
                foreach (var participantAge in eventDto.ParticipantsAges)
                {
                    publika += participantAge.Label + ", ";
                }
                if (publika != "")
                {
                    publika = publika.Substring(0, publika.LastIndexOf(","));
                }


                sb.Append($@"
                                <h3 style=""font-family: 'Arial'; display: inline"">Publika: </h3>
                                <span style=""font-family: 'Arial'"">{publika}</span>
                                <br>
                                <br>
                ");


                sb.Append($@"
                                <h3 style=""font-family: 'Arial'; display: inline"">Vrsta događaja: </h3>
                                <span style=""font-family: 'Arial'"">{eventDto.Type}</span>
                                <br>
                                <br>
                                <h3 style=""font-family: 'Arial'"">Sažetak:</h3>
                                <p style=""font-family: 'Arial'; text-align: justify; line-height:25px"">{eventDto.Summary}</p>
                                <br>
                                <br>
                                <h3 style=""font-family: 'Arial'"">Biografija:</h3>

                ");



                foreach (var lecturer in eventDto.Lecturers)
                {
                    sb.Append(@$"
                                <p style=""font-family: 'Arial'; text-align: justify; line-height:25px"">{lecturer.FirstName} {lecturer.LastName}</p>
                                <p style=""font-family: 'Arial'; text-align: justify; line-height:25px"">{lecturer.Resume}</p>
                                <br>                              
                    ");
                }

                sb.AppendLine("<div style=\"page-break-before: always;\"></div>");
            }

            sb.Length = sb.Length - 1;      //ovo ne radi, provjerit zašto


            return sb.ToString();
        }


        public async Task<byte[]> GenerateFestivalTable(int? festivalYearId = null)
        {
            var blobalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Landscape,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 20, Bottom = 20, Left = 15, Right = 15 },


                //DocumentTitle = "PdfReport",

            };

            var events = await _eventsService.GetEvents(festivalYearId);

            string htmlContent = CreateFestivalTableHtmlContent(events);

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlContent,
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "styles.css") },
            };

            var pdf = new HtmlToPdfDocument
            {
                GlobalSettings = blobalSettings,
                Objects = { objectSettings },
            };

            var file = _converter.Convert(pdf);

            return file;
        }


        private string CreateFestivalTableHtmlContent(IEnumerable<EventDto> events)
        {

            var sb = new StringBuilder();

            foreach (var eventDto in events)
            {
                sb.Append(@$"
                                <h3 style=""font-family: 'Arial'"">{eventDto.Title}</h3>
                                <h3 style=""font-family: 'Arial'; display: inline"">Datum:</h3>
                                <span style=""font-family: 'Arial'; margin-left:5px"">08. travanj 2019., 10.30 – 11.00</span>
                                <br>
                                <br>
                               
                               


                ");

            }

            return "<table border=\"1\" style=\"width: 100%;border-collapse:collapse;font-family: 'Arial'\">\r\n\r\n    <tbody>\r\n         <tr >\r\n            <th  colspan=\"4\"  style=\"text-align: left; padding:10px;  font-weight: bold;\"  >8. TRAVNJA, PONEDJELJAK</th>\r\n        </tr>\r\n        <tr>\r\n             <th colspan=\"4\"  style=\"text-align: left;padding:10px;  font-weight: bold; \" >Tehnički muzej „Nikola Tesla“, Savska cesta 18</th>\r\n        </tr>\r\n        <tr>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\">10.00 – 20.00</td>\r\n            <td style=\"width: 15%;text-align: left;padding:10px\" >TMNT, Velika dvorana</td>\r\n            <td style=\"width: 15%;text-align: left;padding:10px\" >Izložba</td>\r\n            <td  style=\"text-align: left;padding:10px\" >Osvajanje svemira, izložba HDOFM</td>\r\n        </tr>\r\n        <tr>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\">11.00 – 11.30</td>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\" >TMNT, Velika dvorana</td>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\" >Prezentacija <br> S2, S3</td>\r\n            <td  style=\"text-align: left;padding:10px\" >Dinamika plodonošenja i očuvanje genofonda hrasta lužnjaka (Quercus robur L.) i obične bukve (Fagus sylvatica L.) u svijetu klimatskih promijena, Anđelina Gavranović\r\nProjekt Hrvatske zaklade za znanost (2018-2022.), voditelj projekta: Mladen Ivanković</td>\r\n        </tr>\r\n            <tr>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\">10.00 – 20.00</td>\r\n            <td style=\"width: 15%;text-align: left;padding:10px\" >TMNT, Velika dvorana</td>\r\n            <td style=\"width: 15%;text-align: left;padding:10px\" >Izložba</td>\r\n            <td  style=\"text-align: left;padding:10px\" >Osvajanje svemira, izložba HDOFM</td>\r\n        </tr>\r\n        <tr>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\">11.00 – 11.30</td>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\" >TMNT, Velika dvorana</td>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\" >Prezentacija <br> S2, S3</td>\r\n            <td  style=\"text-align: left;padding:10px\" >Dinamika plodonošenja i očuvanje genofonda hrasta lužnjaka (Quercus robur L.) i obične bukve (Fagus sylvatica L.) u svijetu klimatskih promijena, Anđelina Gavranović\r\nProjekt Hrvatske zaklade za znanost (2018-2022.), voditelj projekta: Mladen Ivanković</td>\r\n        </tr>\r\n        <tr>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\">10.00 – 20.00</td>\r\n            <td style=\"width: 15%;text-align: left;padding:10px\" >TMNT, Velika dvorana</td>\r\n            <td style=\"width: 15%;text-align: left;padding:10px\" >Izložba</td>\r\n            <td  style=\"text-align: left;padding:10px\" >Osvajanje svemira, izložba HDOFM</td>\r\n        </tr>\r\n        <tr>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\">11.00 – 11.30</td>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\" >TMNT, Velika dvorana</td>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\" >Prezentacija <br> S2, S3</td>\r\n            <td  style=\"text-align: left;padding:10px\" >Dinamika plodonošenja i očuvanje genofonda hrasta lužnjaka (Quercus robur L.) i obične bukve (Fagus sylvatica L.) u svijetu klimatskih promijena, Anđelina Gavranović\r\nProjekt Hrvatske zaklade za znanost (2018-2022.), voditelj projekta: Mladen Ivanković</td>\r\n        </tr>\r\n        <tr>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\">10.00 – 20.00</td>\r\n            <td style=\"width: 15%;text-align: left;padding:10px\" >TMNT, Velika dvorana</td>\r\n            <td style=\"width: 15%;text-align: left;padding:10px\" >Izložba</td>\r\n            <td  style=\"text-align: left;padding:10px\" >Osvajanje svemira, izložba HDOFM</td>\r\n        </tr>\r\n        <tr>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\">11.00 – 11.30</td>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\" >TMNT, Velika dvorana</td>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\" >Prezentacija <br> S2, S3</td>\r\n            <td  style=\"text-align: left;padding:10px\" >Dinamika plodonošenja i očuvanje genofonda hrasta lužnjaka (Quercus robur L.) i obične bukve (Fagus sylvatica L.) u svijetu klimatskih promijena, Anđelina Gavranović\r\nProjekt Hrvatske zaklade za znanost (2018-2022.), voditelj projekta: Mladen Ivanković</td>\r\n        </tr>\r\n        <tr>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\">10.00 – 20.00</td>\r\n            <td style=\"width: 15%;text-align: left;padding:10px\" >TMNT, Velika dvorana</td>\r\n            <td style=\"width: 15%;text-align: left;padding:10px\" >Izložba</td>\r\n            <td  style=\"text-align: left;padding:10px\" >Osvajanje svemira, izložba HDOFM</td>\r\n        </tr>\r\n        <tr>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\">11.00 – 11.30</td>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\" >TMNT, Velika dvorana</td>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\" >Prezentacija <br> S2, S3</td>\r\n            <td  style=\"text-align: left;padding:10px\" >Dinamika plodonošenja i očuvanje genofonda hrasta lužnjaka (Quercus robur L.) i obične bukve (Fagus sylvatica L.) u svijetu klimatskih promijena, Anđelina Gavranović\r\nProjekt Hrvatske zaklade za znanost (2018-2022.), voditelj projekta: Mladen Ivanković</td>\r\n        </tr>\r\n        <tr>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\">10.00 – 20.00</td>\r\n            <td style=\"width: 15%;text-align: left;padding:10px\" >TMNT, Velika dvorana</td>\r\n            <td style=\"width: 15%;text-align: left;padding:10px\" >Izložba</td>\r\n            <td  style=\"text-align: left;padding:10px\" >Osvajanje svemira, izložba HDOFM</td>\r\n        </tr>\r\n        <tr>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\">11.00 – 11.30</td>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\" >TMNT, Velika dvorana</td>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\" >Prezentacija <br> S2, S3</td>\r\n            <td  style=\"text-align: left;padding:10px\" >Dinamika plodonošenja i očuvanje genofonda hrasta lužnjaka (Quercus robur L.) i obične bukve (Fagus sylvatica L.) u svijetu klimatskih promijena, Anđelina Gavranović\r\nProjekt Hrvatske zaklade za znanost (2018-2022.), voditelj projekta: Mladen Ivanković</td>\r\n        </tr>\r\n        <tr>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\">10.00 – 20.00</td>\r\n            <td style=\"width: 15%;text-align: left;padding:10px\" >TMNT, Velika dvorana</td>\r\n            <td style=\"width: 15%;text-align: left;padding:10px\" >Izložba</td>\r\n            <td  style=\"text-align: left;padding:10px\" >Osvajanje svemira, izložba HDOFM</td>\r\n        </tr>\r\n        <tr>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\">11.00 – 11.30</td>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\" >TMNT, Velika dvorana</td>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\" >Prezentacija <br> S2, S3</td>\r\n            <td  style=\"text-align: left;padding:10px\" >Dinamika plodonošenja i očuvanje genofonda hrasta lužnjaka (Quercus robur L.) i obične bukve (Fagus sylvatica L.) u svijetu klimatskih promijena, Anđelina Gavranović\r\nProjekt Hrvatske zaklade za znanost (2018-2022.), voditelj projekta: Mladen Ivanković</td>\r\n        </tr>\r\n        <tr>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\">10.00 – 20.00</td>\r\n            <td style=\"width: 15%;text-align: left;padding:10px\" >TMNT, Velika dvorana</td>\r\n            <td style=\"width: 15%;text-align: left;padding:10px\" >Izložba</td>\r\n            <td  style=\"text-align: left;padding:10px\" >Osvajanje svemira, izložba HDOFM</td>\r\n        </tr>\r\n        <tr>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\">11.00 – 11.30</td>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\" >TMNT, Velika dvorana</td>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\" >Prezentacija <br> S2, S3</td>\r\n            <td  style=\"text-align: left;padding:10px\" >Dinamika plodonošenja i očuvanje genofonda hrasta lužnjaka (Quercus robur L.) i obične bukve (Fagus sylvatica L.) u svijetu klimatskih promijena, Anđelina Gavranović\r\nProjekt Hrvatske zaklade za znanost (2018-2022.), voditelj projekta: Mladen Ivanković</td>\r\n        </tr>\r\n        <tr>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\">10.00 – 20.00</td>\r\n            <td style=\"width: 15%;text-align: left;padding:10px\" >TMNT, Velika dvorana</td>\r\n            <td style=\"width: 15%;text-align: left;padding:10px\" >Izložba</td>\r\n            <td  style=\"text-align: left;padding:10px\" >Osvajanje svemira, izložba HDOFM</td>\r\n        </tr>\r\n        <tr>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\">11.00 – 11.30</td>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\" >TMNT, Velika dvorana</td>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\" >Prezentacija <br> S2, S3</td>\r\n            <td  style=\"text-align: left;padding:10px\" >Dinamika plodonošenja i očuvanje genofonda hrasta lužnjaka (Quercus robur L.) i obične bukve (Fagus sylvatica L.) u svijetu klimatskih promijena, Anđelina Gavranović\r\nProjekt Hrvatske zaklade za znanost (2018-2022.), voditelj projekta: Mladen Ivanković</td>\r\n        </tr>\r\n        <tr>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\">10.00 – 20.00</td>\r\n            <td style=\"width: 15%;text-align: left;padding:10px\" >TMNT, Velika dvorana</td>\r\n            <td style=\"width: 15%;text-align: left;padding:10px\" >Izložba</td>\r\n            <td  style=\"text-align: left;padding:10px\" >Osvajanje svemira, izložba HDOFM</td>\r\n        </tr>\r\n        <tr>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\">11.00 – 11.30</td>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\" >TMNT, Velika dvorana</td>\r\n            <td  style=\"width: 15%;text-align: left;padding:10px\" >Prezentacija <br> S2, S3</td>\r\n            <td  style=\"text-align: left;padding:10px\" >Dinamika plodonošenja i očuvanje genofonda hrasta lužnjaka (Quercus robur L.) i obične bukve (Fagus sylvatica L.) u svijetu klimatskih promijena, Anđelina Gavranović\r\nProjekt Hrvatske zaklade za znanost (2018-2022.), voditelj projekta: Mladen Ivanković</td>\r\n        </tr>\r\n        \r\n        \r\n    </tbody>\r\n</table>";




        }



    }
}
