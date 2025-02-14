using BusinessObject;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
	public class ProductRepository
	{
		private readonly ProductDAO _productDAO = new ProductDAO();
		public List<Products> GetAllProducts()
		{
			return ProductDAO.GetProducts();	
		}
		public  Products FindProductById(int id)
		{
			return ProductDAO.FindProductById(id);
		}
		public  void SaveProduct(Products products)
		{
			ProductDAO.SaveProduct(products);
		}
		public  void  DeleteProduct(Products products
			)
		{
		 ProductDAO.DeleteProduct(products);
		}
		public  void  UpdateProduct ( Products p)
		{
			ProductDAO.UpdateProduct(p);
		}
	}
}
