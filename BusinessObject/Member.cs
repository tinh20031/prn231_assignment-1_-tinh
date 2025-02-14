using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject
{
	public class Member
    {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int MemberId { get; set; }
		public string? Email { get; set; }
		public string CompanyName { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
		
        [Column("PassWord")]
        public string Password { get; set; } 

	}
}
