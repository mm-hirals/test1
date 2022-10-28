using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace MidCapERP.Core.CommonHelper
{
    public static class CommonMethod
    {
        public static decimal GetCalculatedPrice(decimal CostPrice, decimal? RetailerPercentage, int? RoundTo)
        {
            decimal retailerPrice = RetailerPercentage > 0 ? CostPrice + (CostPrice * Convert.ToDecimal(RetailerPercentage) / 100) : CostPrice;
            retailerPrice = RoundTo > 0 ? (decimal)(RoundTo) * Math.Round((decimal)(retailerPrice / RoundTo)) : retailerPrice;
            return Math.Round(Math.Round(retailerPrice, 2));
        }

        public static async Task<Stream> GeneratePDF(string html)
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });
            await using var page = await browser.NewPageAsync();
            await page.EmulateMediaTypeAsync(MediaType.Screen);
            var renderedString = html;
            await page.SetContentAsync(renderedString.ToString());
            var pdfContent = await page.PdfStreamAsync(new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true
            });
            return pdfContent;
        }
    }
}