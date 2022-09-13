using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("OTPLogin")]
	public class OTPLogin
	{
		[Key]
		public long Id { get; set; }
		public string PhoneNumber { get; set; }
		public string OTP { get; set; }
		public DateTime ExpiryTime { get; set; }
	}
}
