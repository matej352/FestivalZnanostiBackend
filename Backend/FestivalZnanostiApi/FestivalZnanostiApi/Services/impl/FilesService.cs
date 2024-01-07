using DinkToPdf;
using DinkToPdf.Contracts;
using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.Enums;
using FestivalZnanostiApi.Models;
using FestivalZnanostiApi.Servicess;
using System.Globalization;
using System.Text;


namespace FestivalZnanostiApi.Services.impl
{
    public class FilesService : IFilesService
    {

        private readonly IConverter _converter;
        private readonly IEventsService _eventsService;
        private readonly IFestivalYearService _festivalYearService;
        private readonly ILocationService _locationService;

        public FilesService(IConverter converter, IEventsService eventsService, IFestivalYearService festivalYearService, ILocationService location)
        {
            _converter = converter;
            _eventsService = eventsService;
            _festivalYearService = festivalYearService;
            _locationService = location;
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

            var events = await _eventsService.GetPdfEvents(festivalYearId);

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

        private string CreateEventsSummaryHtmlContent(IEnumerable<PdfEventDto> events)
        {
            var sortedEvents = events
                                     .OrderBy(e => e.TimeSlots.Min(ts => ts.Start))
                                     .Select(e => new EventForSummariesPdf
                                     {
                                         Id = e.Id,
                                         Title = e.Title,
                                         Location = e.Location,
                                         ParticipantsAges = e.ParticipantsAges,
                                         Type = e.Type,
                                         Summary = e.Summary,
                                         Lecturers = e.Lecturers,
                                         // Transform TimeSlots into TimePeriods
                                         TimePeriods = e.TimeSlots
                                                                .GroupBy(ts => ts.Start.Date)
                                                                .Select(group => new
                                                                {
                                                                    Start = group.Min(ts => ts.Start),
                                                                    End = group.Max(ts => ts.Start.Add(TimeSpan.FromMinutes(e.Location.EventDuration)))
                                                                })
                                                                .OrderBy(ts => ts.Start)
                                                                .Select(ts => $"{ts.Start.ToString("dd. MMMM yyyy., HH:mm", new CultureInfo("hr-HR"))} - {ts.End.ToString("HH:mm")}")
                                                                .ToList()
                                     })
                                     .ToList();

            var sb = new StringBuilder();

            int counter = 0;

            foreach (var sortedEvent in sortedEvents)
            {
                counter++;

                sb.Append(@$"
                                <h3 style=""font-family: 'Arial'"">{sortedEvent.Title}</h3>
                                <div style=""display: flex; font-family: 'Arial';"">
                                    <div style=""font-size: 1.17em;font-weight: bold;margin-right:5px;"">Datum: </div>
                                    <div style=""display: flex; flex-direction: column; padding-top: 2.5px"">
                ");

                foreach (var timePeriod in sortedEvent.TimePeriods)
                {
                    sb.Append(@$"
                                <span style=""display: block"" >{timePeriod}</span>
                    ");
                }

                sb.Append(@$"
                                   </div>
                                </div>
                                <br>
                                <br>
                    ");



                var lokacija = "";
                if (sortedEvent.Location.ParentLocation is not null)
                {
                    lokacija += sortedEvent.Location.ParentLocation.Name + " | ";
                }
                lokacija += sortedEvent.Location.Name;

                sb.Append(@$"
                                <h3 style=""font-family: 'Arial'; display: inline"">Lokacija: </h3>
                                <span style=""font-family: 'Arial'"">{lokacija}</span>
                                <br>
                                <br>
                               


                ");

                var publika = "";
                foreach (var participantAge in sortedEvent.ParticipantsAges)
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
                                <span style=""font-family: 'Arial'"">{sortedEvent.Type}</span>
                                <br>
                                <br>
                                <h3 style=""font-family: 'Arial'"">Sažetak:</h3>
                                <p style=""font-family: 'Arial'; text-align: justify; line-height:25px"">{sortedEvent.Summary}</p>
                                <br>
                                <br>
                                <h3 style=""font-family: 'Arial'"">Biografija:</h3>

                ");



                foreach (var lecturer in sortedEvent.Lecturers)
                {
                    sb.Append(@$"
                                <p style=""font-family: 'Arial'; text-align: justify; line-height:25px"">{lecturer.FirstName} {lecturer.LastName}</p>
                                <p style=""font-family: 'Arial'; text-align: justify; line-height:25px"">{lecturer.Resume}</p>
                                <br>                              
                    ");
                }


                if (counter < events.Count())
                {
                    sb.AppendLine("<div style=\"page-break-before: always;\"></div>");
                }

            }


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

            var events = await _eventsService.GetPdfEvents(festivalYearId);
            var locations = await _locationService.GetLocations();

            FestivalYearDto festivalYear;
            if (festivalYearId != null)
            {
                festivalYear = await _festivalYearService.GetFestivalYear((int)festivalYearId);
            }
            else
            {
                festivalYear = await _festivalYearService.GetActiveFestivalYear();
            }



            string htmlContent = CreateFestivalTableHtmlContent(events, locations, festivalYear.StartDate, festivalYear.EndDate);

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


        private string CreateFestivalTableHtmlContent(IEnumerable<PdfEventDto> events, IEnumerable<LocationDto> locations, DateTime startDate, DateTime endDate)
        {




            var eventGroups = events.SelectMany(e => e.TimeSlots.Select(
                ts => new
                {
                    Event = e,
                    Date = ts.Start.Date,
                    ParentLocationId = e.Location.ParentLocationId ?? e.Location.Id
                }
                )).GroupBy(e => e.Date)
                                  .ToDictionary(
                                        group => group.Key,
                                        group => group
                                            .GroupBy(e => e.ParentLocationId)
                                            .ToDictionary(
                                                subGroup => subGroup.Key,
                                                subGroup => subGroup.Select(item => item.Event).Distinct().ToList()
                                            )
                                    );


            var sb = new StringBuilder();


            foreach (var dateEntry in eventGroups)
            {

                foreach (var parentLocationEntry in dateEntry.Value)
                {

                    var eventsForPdf = new List<EventForTablePdf>();

                    sb.Append(@$"
                                <table border=""1"" style=""width: 100%;margin-bottom: 30px;border-collapse:collapse;font-family: 'Arial'"">
                                    <tbody>                                 
                      ");

                    string formattedDate = dateEntry.Key.ToString("d. MMMM, dddd", new CultureInfo("hr-HR")).ToUpper();

                    sb.Append(@$"
                                <tr>
                                     <th  colspan=""4""  style=""text-align: left; padding:10px;  font-weight: bold;"">
                                            {formattedDate}
                                     </th>     
                                </tr>
                                <tr>
                                     <th colspan=""4""  style=""text-align: left;padding:10px;  font-weight: bold; "" >
                                            {locations.FirstOrDefault(l => l.Id == parentLocationEntry.Key)?.Name}
                                     </th>
                                </tr>
        
                      ");



                    // Order events by TimeSlots
                    var orderedEvents = parentLocationEntry.Value.OrderBy(eventEntry =>
                    {
                        var timeSlots = eventEntry.TimeSlots.Where(ts => ts.Start.Date == dateEntry.Key);
                        var startTime = timeSlots.Min(ts => ts.Start.TimeOfDay);
                        var endTime = timeSlots.Max(ts => ts.Start.Add(TimeSpan.FromMinutes(eventEntry.Location.EventDuration)).TimeOfDay);
                        return startTime;
                    });

                    foreach (var eventEntry in orderedEvents)
                    {

                        // Calculate TimePeriod
                        var timeSlots = eventEntry.TimeSlots.Where(ts => ts.Start.Date == dateEntry.Key);
                        var startTime = timeSlots.Min(ts => ts.Start.TimeOfDay);
                        var endTime = timeSlots.Max(ts => ts.Start.Add(TimeSpan.FromMinutes(eventEntry.Location.EventDuration)).TimeOfDay);
                        var timePeriod = $"{startTime:hh\\:mm} - {endTime:hh\\:mm}";


                        // Populate eventsForPdf list
                        eventsForPdf.Add(new EventForTablePdf
                        {
                            Id = eventEntry.Id,
                            Title = eventEntry.Title,
                            Type = eventEntry.Type,
                            Location = eventEntry.Location.Name,
                            Lecturers = eventEntry.Lecturers,
                            ParticipantsAges = eventEntry.ParticipantsAges,
                            TimePeriod = timePeriod
                        });
                    }


                    foreach (var eventForPdf in eventsForPdf)
                    {

                        string autoriOrPredavaciOrVoditelji = "";
                        string participantAgeLabels = "";

                        switch (eventForPdf.Type)
                        {
                            case "Prezentacija":
                                autoriOrPredavaciOrVoditelji += "voditelj/i: ";
                                break;
                            case "Radionica":
                                autoriOrPredavaciOrVoditelji += "voditelj/i: ";
                                break;
                            case "Predavanje":
                                autoriOrPredavaciOrVoditelji += "predavač/i: ";
                                break;
                            case "Izložba":
                                autoriOrPredavaciOrVoditelji += "autor/i: ";
                                break;
                        }

                        var counter = 0;

                        foreach (var lecturer in eventForPdf.Lecturers)
                        {
                            counter++;
                            autoriOrPredavaciOrVoditelji += $"{lecturer.FirstName} {lecturer.LastName}";
                            if (counter < eventForPdf.Lecturers.Count)
                            {
                                autoriOrPredavaciOrVoditelji += ", ";
                            }
                        }

                        if (eventForPdf.Type != "Izložba")
                        {
                            counter = 0;
                            foreach (var participantAge in eventForPdf.ParticipantsAges)
                            {
                                counter++;
                                participantAgeLabels += $"{participantAge.Label}";
                                if (counter < eventForPdf.ParticipantsAges.Count)
                                {
                                    participantAgeLabels += ", ";
                                }
                            }
                        }




                        sb.Append(@$"
                                  <tr>
                                        <td  style=""width: 15%;text-align: left;padding:10px"">{eventForPdf.TimePeriod}</td>        
                                        <td style=""width: 15%;text-align: left;padding:10px"" >{eventForPdf.Location}</td>       
                                        <td style=""width: 15%;text-align: left;padding:10px"" >{eventForPdf.Type} <br> {participantAgeLabels}</td>    
                                        <td style =""text - align: left; padding: 10px"" > {eventForPdf.Title}, {autoriOrPredavaciOrVoditelji}</ td >
                                  </tr>
                    ");




                    }

                    sb.Append(@$"
                                    </tbody>
                                </table>                               
                      ");

                }
            }

            return sb.ToString();
        }






        private class EventForTablePdf
        {
            public int Id { get; set; }

            public string Title { get; set; }

            public string Type { get; set; }

            public string Location { get; set; }

            public List<LecturerDto> Lecturers { get; set; }

            public List<ParticipantsAgeDto> ParticipantsAges { get; set; }

            public string TimePeriod { get; set; }
        }

        private class EventForSummariesPdf
        {
            public int Id { get; set; }

            public string Title { get; set; }

            public string Summary { get; set; }

            public string Type { get; set; }

            public Location Location { get; set; }

            public List<LecturerDto> Lecturers { get; set; }

            public List<ParticipantsAgeDto> ParticipantsAges { get; set; }

            public List<string> TimePeriods { get; set; }
        }
    }
}
