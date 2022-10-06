namespace MidCapERP.BusinessLogic.Services.PriceCalculation
{
    public interface IPriceCalculationService
    {
        public decimal GetCalculatedPrice(decimal CostPrice, decimal? RetailerPercentage, int RoundTo = 50);
    }
}