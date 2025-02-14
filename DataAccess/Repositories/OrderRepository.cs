using BusinessObject;
using DataAccess.DAO;
using System.Collections.Generic;

namespace DataAccess.Repositories
{
	public class OrderRepository
	{
		public List<Order> GetAllOrders() => OrderDAO.GetOrders();

		public Order GetOrderById(int id) => OrderDAO.FindOrderById(id);

		public void AddOrder(Order order) => OrderDAO.SaveOrder(order);

		public void UpdateOrder(Order order) => OrderDAO.UpdateOrder(order);

		public void RemoveOrder(int id) => OrderDAO.DeleteOrder(id);
	}
}
