using DinkToPdf;
using DinkToPdf.Contracts;
using PdfSharpCore.Pdf;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace FestivalZnanostiApi.Services.impl
{
    public class FilesService : IFilesService
    {

        private readonly IConverter _converter;


        public FilesService(IConverter converter)
        {
            _converter = converter;
        }


        public byte[] GenerateEventSummary(int EventId)
        {

            var blobalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 30, Bottom = 30, Left = 25, Right = 25 },
                DocumentTitle = "PdfReport",

            };



            string htmlContent = "<h3 style=\"font-family: 'Arial'\">Mikropropagacija šumskih vrsta drveća u Hrvatskom šumarskom institutu</h3>\r\n    <h3 style=\"font-family: 'Arial'; display: inline\">Datum:</h3>\r\n    <span style=\"font-family: 'Arial'; margin-left:5px\">08. travanj 2019., 10.30 – 11.00</span>\r\n<br>\r\n<br>\r\n<h3 style=\"font-family: 'Arial'; display: inline\">Lokacija: </h3>\r\n<span style=\"font-family: 'Arial'\">Tehnički muzej „Nikola Tesla“, Savska cesta 18</span>\r\n<br>\r\n<br>\r\n<h3 style=\"font-family: 'Arial'; display: inline\">Publika: </h3>\r\n<span style=\"font-family: 'Arial'\">S1, S2, S3</span>\r\n<br>\r\n<br>\r\n<h3 style=\"font-family: 'Arial'; display: inline\">Vrsta događaja: </h3>\r\n<span style=\"font-family: 'Arial'\">Radionica</span>\r\n<br>\r\n<br>\r\n<h3 style=\"font-family: 'Arial'\">Sažetak:</h3>\r\n<p style=\"font-family: 'Arial'; text-align: justify; line-height:25px\">Jedan od temeljnih zadataka šumara je izmjeriti stabla u šumi. To znači da se stablima prvenstveno mjere ukupna visina te debljina debla (prsni promjer stabla) te izračunavaju veličine poput obujma debla, grana, krošnje itd. Na radionici će biti pokazano kako se šumarskim instrumentima mjere navedene dimenzije. Upoznaj se s tehnikama mjerenja, uzmi u ruke promjerku, visinomjer te probaj sam/sama izmjeriti te izračunati dimenzije stabala.</p>\r\n<br>\r\n<br>\r\n<h3 style=\"font-family: 'Arial'\">Biografija:</h3>\r\n<p style=\"font-family: 'Arial'; text-align: justify; line-height:25px\">Sanja Bogunović rođena je 03. prosinca 1987. godine u Sisku, Republika Hrvatska. Osnovnu školu završila je u Jabukovcu, a opću gimnaziju u Srednjoj školi Petrinja. Nakon toga upisala je preddiplomski studij šumarstva na Šumarskom fakultetu, Sveučilište u Zagrebu, i po završetku istoga upisuje diplomski studij, smjer: Uzgajanje i uređivanje šuma s lovnim gospodarenjem. 2012. godine diplomirala je na temu „Varijabilnost fenofaza cvjetanja poljskog jasena (Fraxinus angustifolia Vahl) u klonskoj sjemenskoj plantaži“. Od studenog 2013. do listopada 2014. godine radila je kao asistentica na projektu financiranom iz sredstava Europske unije „E4E – Education for Employment“ u Centru za šljivu i kesten, Donja Bačuga. 1. siječnja 2015. godine započinje raditi na Hrvatskom šumarskom institutu, Zavod za genetiku, oplemenjivanje šumskog drveća i sjemenarstvo, kao asistent na projektu Hrvatske zaklade za znanost te upisuje poslijediplomski doktorski studij Šumarstvo i drvna tehnologija na Šumarskom fakultetu u Zagrebu. Bila je suradnik na projektu HRZZ-a „Conservation of genetic resources of forest trees in light of climate changes“ te sudjeluje na projektima OKFŠ-a „Osnivanje pokusa za provođenje uzgojnih i genetsko meliorativnih zahvata u mladim sastojinama hrasta lužnjaka kao temelj za gospodarenje\r\nbudućim sjemenskim sastojinama“ i HRZZ-a „Dinamika plodonošenja i očuvanja genofonda hrasta lužnjaka (Quercus robur L.) i obične bukve (Fagus sylvatica L.) u svjetlu klimatskih promjena“. Članica je Hrvatskog šumarskog društva i Hrvatske udruge za arborikulturu.</p><div style=\"page-break-before: always;\">\r\nIde gas ovo je na drugoj stranici\r\n</div>";
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
    }
}
