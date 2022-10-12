﻿using System.ComponentModel;

namespace MidCapERP.Dto.Product
{
    public class ProductResponseDto
    {
        public Int64 ProductId { get; set; }
        public int CategoryId { get; set; }

        [DisplayName("Category Name")]
        public string? CategoryName { get; set; }

        [DisplayName("Product Title")]
        public string ProductTitle { get; set; }

        [DisplayName("Model No")]
        public string ModelNo { get; set; }

        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Depth { get; set; }
        public decimal? FabricNeeded { get; set; }
        public bool IsVisibleToWholesalers { get; set; }
        public decimal TotalDaysToPrepare { get; set; }
        public string? Features { get; set; }
        public string? Comments { get; set; }

        [DisplayName("Cost Price")]
        public decimal CostPrice { get; set; }

        public string? QRImage { get; set; }
        public int TenantId { get; set; }

        [DisplayName("Publish")]
        public int Status { get; set; }

        public int CreatedBy { get; set; }

        [DisplayName("Created By")]
        public string CreatedByName { get; set; }

        public DateTime CreatedDate { get; set; }

        [DisplayName("Created Date")]
        public string CreatedDateFormat => CreatedDate.ToLongDateString();

        public DateTime CreatedUTCDate { get; set; }
        public int? UpdatedBy { get; set; }

        [DisplayName("Last Modified By")]
        public string? UpdatedByName { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [DisplayName("Last Modified On")]
        public string UpdatedDateFormat => UpdatedDate != null && UpdatedDate.HasValue ? UpdatedDate.Value.ToLongDateString() : CreatedDate.ToLongDateString();

        public DateTime? UpdatedUTCDate { get; set; }

        /// <summary>
        /// Remarks : Do not change the method Name or Properties. Check the PagedList.cs to get referance of the method
        /// </summary>
        /// <param name="orderbyColumn">Order by column name</param>
        /// <returns>actual database column name</returns>
        public string MapOrderBy(string orderbyColumn)
        {
            switch (orderbyColumn)
            {
                case "updatedDateFormat":
                    return "UpdatedDate";

                case "createdDateFormat":
                    return "CreatedDate";

                case "createdByName":
                    return "CreatedBy";

                case "updatedByName":
                    return "UpdatedBy";

                default:
                    return orderbyColumn;
            };
        }
    }
}