using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Infrastructure.Identity.Models
{
    public class TokenAPIRequest
    {
        /// <summary>
        /// The phone No which is used for login
        /// </summary>
        [Required]
        [JsonProperty("phoneNo")]
        public string PhoneNo { get; set; }

        /// <summary>
        /// OTP which is used for login
        /// </summary>
        [Required]
        [JsonProperty("otp")]
        public string OTP { get; set; }

        /// <summary>
        /// MobileDeviceId which is used for AspNetUsers
        /// </summary>
        [Required]
        [JsonProperty("MobileDeviceId")]
        public string MobileDeviceId { get; set; }
    }
}
