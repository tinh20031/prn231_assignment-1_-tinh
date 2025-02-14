using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BusinessObject
{
	public class Products
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ProductId { get; set; }

	public string ProductName { get; set; }
	public decimal Weight { get; set; }
		[Column(TypeName ="money")]
		public decimal UnitPrice { get; set; }
		public int UnitsInStock { get; set; }

		[Required]
		public int CategoryId { get; set; }


		[ForeignKey("CategoryId")]
		public Category? category { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();



    }
}
