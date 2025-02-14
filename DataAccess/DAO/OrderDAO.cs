using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.DAO
{
	public class OrderDAO
	{

		public static List<Order> GetOrders()
		{
			using (var context = new eStoreDbContext())
			{
				return context.orders.Include(o => o.Member).ToList();
			}
		}

		// ✅ Tìm đơn hàng theo ID
		public static Order FindOrderById(int id)
		{
			using (var context = new eStoreDbContext())
			{
				return context.orders.Include(o => o.Member).SingleOrDefault(o => o.OrderId == id);
			}
		}

		// ✅ Thêm đơn hàng mới
		public static void SaveOrder(Order order)
		{
			using (var context = new eStoreDbContext())
			{
				context.orders.Add(order);
				context.SaveChanges();
			}
		}

		// ✅ Cập nhật thông tin đơn hàng
		public static void UpdateOrder(Order order)
		{
			using (var context = new eStoreDbContext())
			{
				var existingOrder = context.orders.Find(order.OrderId);
				if (existingOrder != null)
				{
					context.Entry(existingOrder).CurrentValues.SetValues(order);
					context.SaveChanges();
				}
				else
				{
					throw new Exception($"Order with ID {order.OrderId} not found.");
				}
			}
		}

		// ✅ Xóa đơn hàng
		public static void DeleteOrder(int id)
		{
			using (var context = new eStoreDbContext())
			{
				var order = context.orders.Find(id);
				if (order != null)
				{
					context.orders.Remove(order);
					context.SaveChanges();
				}
				else
				{
					throw new Exception($"Order with ID {id} not found.");
				}
			}
		}



        public static List<SalesReport> GetSalesReport(DateTime startDate, DateTime endDate)
        {
            using (var context = new eStoreDbContext())
            {
                var salesData = context.orderDetails
                    .Where(od => od.Order.OrderDate >= startDate && od.Order.OrderDate <= endDate)
                    .GroupBy(od => od.Order.OrderDate.Date)
                    .Select(g => new SalesReport
                    {
                        Date = g.Key,
                        TotalOrders = g.Select(od => od.OrderId).Distinct().Count(), // Đếm số đơn hàng duy nhất
                        TotalRevenue = g.Sum(od => od.UnitPrice * od.Quantity) // Tổng doanh thu
                    })
                    .OrderBy(s => s.Date)
                    .ToList();

                return salesData;
            }
        }
    }
}

