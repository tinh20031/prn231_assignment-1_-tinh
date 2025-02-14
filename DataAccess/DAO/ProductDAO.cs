using BusinessObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class ProductDAO
    {
        public static List<Products> GetProducts()
        {
            var ListProduct = new List<Products>();
            try
            {
                using (var context = new eStoreDbContext())
                {
                    ListProduct = context.products.Include( p => p.category).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return ListProduct;
        }
		public static Products FindProductById(int ProId)
		{
			using (var context = new eStoreDbContext())
			{
				return context.products
					.Include(p => p.category) // ✅ Load cả Category
					.SingleOrDefault(x => x.ProductId == ProId);
			}
		}

		public static void SaveProduct (Products p)
        {
            using (var context = new eStoreDbContext())
            {
                
				context.products.Add(p);
                context.SaveChanges();
            }
        }
		public static void UpdateProduct(Products p)
		{
			using (var context = new eStoreDbContext())
			{
				var existingProduct = context.products.Find(p.ProductId);
				if (existingProduct != null) // Kiểm tra sản phẩm có tồn tại không
				{
					context.Entry(existingProduct).CurrentValues.SetValues(p);
					context.SaveChanges(); // ✅ Lưu thay đổi vào database
				}
				else
				{
					throw new Exception("Product not found!");
				}
			}
		}
        public static void DeleteProduct ( Products p)
        {
            using (var context = new eStoreDbContext())
            {
                var p1 = context.products.SingleOrDefault(c => c.ProductId == p.ProductId);
                context.products.Remove(p1);
                context.SaveChanges();
            }
        }
	}
}
