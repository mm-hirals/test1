namespace MidCapERP.BusinessLogic.Services.PriceCalculation
{
    public class PriceCalculationService : IPriceCalculationService
    {
        public decimal GetCalculatedPrice(decimal CostPrice, decimal? RetailerPercentage, int RoundTo = 50)
        {
            decimal retailerPrice = RetailerPercentage > 0 ? CostPrice + ((CostPrice * Convert.ToDecimal(RetailerPercentage)) / 100) : CostPrice;
            retailerPrice = RoundTo * Math.Round(retailerPrice / RoundTo);
            return Math.Round(Math.Round(retailerPrice, 2));
        }
    }
}