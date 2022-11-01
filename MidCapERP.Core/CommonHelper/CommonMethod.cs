using Wkhtmltopdf.NetCore;

namespace MidCapERP.Core.CommonHelper
{
    public class CommonMethod
    {
        private readonly IGeneratePdf _generatePdf;

        public CommonMethod(IGeneratePdf generatePdf)
        {
            _generatePdf = generatePdf;
        }

        public static decimal GetCalculatedPrice(decimal CostPrice, decimal? RetailerPercentage, int? RoundTo)
        {
            decimal retailerPrice = RetailerPercentage > 0 ? CostPrice + (CostPrice * Convert.ToDecimal(RetailerPercentage) / 100) : CostPrice;
            retailerPrice = RoundTo > 0 ? (decimal)(RoundTo) * Math.Round((decimal)(retailerPrice / RoundTo)) : retailerPrice;
            return Math.Round(Math.Round(retailerPrice, 2));
        }

        public byte[] GeneratePDF(string html)
        {
            return _generatePdf.GetPDF(html);
        }
    }
}