using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
