using System.ComponentModel;

namespace MidCapERP.Dto.ProductQuantities
{
    public class ProductQuantitiesResponseDto
    {
        public long ProductQuantityId { get; set; }
        public long ProductId { get; set; }

        [DisplayName("Product Title")]
        public string ProductTitle { get; set; }

        public long CategoryId { get; set; }

        [DisplayName("Category Name")]
        public string CategoryName { get; set; }

        public DateTime QuantityDate { get; set; }

        [DisplayName("Quantity Date")]
        public string QuantityDateFormat => QuantityDate.ToShortDateString();

        public int Quantity { get; set; }
        public long LastModifiedBy { get; set; }

        [DisplayName("Last Modified Name")]
        public string LastModifiedByName { get; set; }

        public DateTime LastModifiedDate { get; set; }

        [DisplayName("Last Modified Date")]
        public string LastModifiedDateFormat => LastModifiedDate.ToLongDateString();

        public DateTime LastModifiedUTCDate { get; set; }

        /// <summary>
        /// Remarks : Do not change the method Name or Properties. Check the PagedList.cs to get referance of the method
        /// </summary>
        /// <param name="orderbyColumn">Order by column name</param>
        /// <returns>actual database column name</returns>
        public string MapOrderBy(string orderbyColumn)
        {
            switch (orderbyColumn)
            {
                case "lastModifiedByName":
                    return "LastModifiedBy";

                case "quantityDateFormat":
                    return "QuantityDate";

                case "lastModifiedDateFormat":
                    return "LastModifiedDate";

                default:
                    return orderbyColumn;
            };
        }
    }
}