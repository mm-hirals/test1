using System.ComponentModel;
using System.Text.Json.Serialization;

namespace MidCapERP.Dto.Interior
{
    public class InteriorResponseDto
    {
        public long CustomerId { get; set; }

        [DisplayName("Customer Type")]
        public int CustomerTypeId { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Email Address")]
        public string EmailId { get; set; }

        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [DisplayName("Alt. Phone Number")]
        public string AltPhoneNumber { get; set; }

        [DisplayName("GST No")]
        public string? GSTNo { get; set; }

        public InteriorResponseDto? Reffered { get; set; }

        [DisplayName("Reffered Name")]
        public string? RefferedName { get; set; }

        [JsonIgnore]
        public decimal Discount { get; set; }

        public bool IsSubscribe { get; set; }

        public int TenantID { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        [DisplayName("Created Date")]
        public string CreatedDateFormat => CreatedDate.ToLongDateString();

        public DateTime CreatedUTCDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
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
                case "1":
                    return "FirstName";

                default:
                    return orderbyColumn;
            };
        }
    }
}