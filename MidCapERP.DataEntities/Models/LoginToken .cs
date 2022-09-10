using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("LoginToken")]
	public class LoginToken
	{
		[Key]
		public long Id { get; set; }
		public string PhoneNumber { get; set; }
		public string OTP { get; set; }
		public DateTime ExpiryTime { get; set; }
	}
}
