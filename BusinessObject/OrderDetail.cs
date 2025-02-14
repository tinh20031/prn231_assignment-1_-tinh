using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections;

namespace BusinessObject
{
	public class OrderDetail
	{
		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderDetailId { get; set; }

        [ForeignKey("Order")]
		
		public int OrderId { get; set; }

		[ForeignKey("Products")]
		public int ProductId { get; set; }
		public decimal UnitPrice { get; set; }
		public int Quantity { get; set; }
		public decimal Discount { get; set; }
		public virtual Order? Order { get; set; }
		public virtual Products? Products { get; set; }	


	}
}
