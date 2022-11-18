using MidCapERP.Dto.Customers;
using MidCapERP.Dto.OrderSet;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.Order
{
    public class OrderResponseDto
    {
        public long OrderId { get; set; }

        [Required]
        [DisplayName("Order No")]
        public string OrderNo { get; set; }

        [Required]
        public long CustomerID { get; set; }

        [DisplayName("Customer Name")]
        public string CustomerName { get; set; }

        [Required]
        [DisplayName("Gross Total")]
        public decimal GrossTotal { get; set; }

        [Required]
        public decimal Discount { get; set; }

        [Required]
        [DisplayName("Order Amount")]
        public decimal TotalAmount { get; set; }

        [Required]
        [DisplayName("Advance Amount")]
        public decimal AdvanceAmount { get; set; }

        [Required]
        [DisplayName("TAX")]
        public decimal GSTTaxAmount { get; set; }

        [Required]
        [DisplayName("Total Amount")]
        public decimal PayableAmount { get; set; }

        [Required]
        public DateTime? DeliveryDate { get; set; }

        [DisplayName("Delivery Date")]
        public string? DeliveryDateFormat => DeliveryDate != null ? DeliveryDate.Value.ToShortDateString() : "";

        public string? Comments { get; set; }

        [Required]
        [DisplayName("Order Status")]
        public int Status { get; set; }

        public long? RefferedBy { get; set; }

        [DisplayName("Reffered Name")]
        public string? RefferedName { get; set; }

        public string PhoneNumber { get; set; }
        public string IpAddress { get; set; }

        public int CreatedBy { get; set; }

        [DisplayName("Salesman Name")]
        public string CreatedByName { get; set; }

        public DateTime CreatedDate { get; set; } 

        [DisplayName("Order Date")]
        public string CreatedDateFormat => CreatedDate.ToLongDateString();

        public DateTime CreatedUTCDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }

        public CustomersResponseDto customersResponseDto { get; set; }
        public List<OrderSetResponseDto> OrderSetResponseDto { get; set; }
        public List<OrderAddressesResponseDto> OrderAddressesResponseDto { get; set; }

        /// <summary>
        /// Remarks : Do not change the method Name or Properties. Check the PagedList.cs to get referance of the method
        /// </summary>
        /// <param name="orderbyColumn">Order by column name</param>
        /// <returns>actual database column name</returns>
        public string MapOrderBy(string orderbyColumn)
        {
            switch (orderbyColumn)
            {
                case "deliveryDateFormat":
                    return "DeliveryDate";

                case "createdDateFormat":
                    return "CreatedDate";

                case "createdByName":
                    return "CreatedBy";

                default:
                    return orderbyColumn;
            };
        }
    }
}