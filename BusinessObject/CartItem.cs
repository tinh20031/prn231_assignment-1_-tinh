using System.ComponentModel.DataAnnotations;

public class CartItem
{
    [Key]
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
}
