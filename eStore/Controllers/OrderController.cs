using BusinessObject;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace eStore.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		private readonly OrderRepository _orderRepository = new OrderRepository();

		
		[HttpGet]
		public IActionResult GetAllOrders()
		{
			var orders = _orderRepository.GetAllOrders();
			return Ok(orders);
		}

		
		[HttpGet("{id}")]
		public IActionResult GetOrderById(int id)
		{
			var order = _orderRepository.GetOrderById(id);
			if (order == null)
			{
				return NotFound($"Order with ID {id} not found.");
			}
			return Ok(order);
		}


		[HttpPost]
		public IActionResult CreateOrder([FromBody] Order order)
		{
			if (order == null)
			{
				return BadRequest("Invalid order data.");
			}

			_orderRepository.AddOrder(order);
			return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderId }, order);
		}

		// ✅ Cập nhật đơn hàng
		[HttpPut("{id}")]
		public IActionResult UpdateOrder(int id, [FromBody] Order order)
		{
			if (order == null || order.OrderId != id)
			{
				return BadRequest("Invalid order data.");
			}

			var existingOrder = _orderRepository.GetOrderById(id);
			if (existingOrder == null)
			{
				return NotFound($"Order with ID {id} not found.");
			}

			_orderRepository.UpdateOrder(order);
			return NoContent(); 
		}

		
		[HttpDelete("{id}")]
		public IActionResult DeleteOrder(int id)
		{
			var existingOrder = _orderRepository.GetOrderById(id);
			if (existingOrder == null)
			{
				return NotFound($"Order with ID {id} not found.");
			}

			_orderRepository.RemoveOrder(id);
			return NoContent(); 
		}
	}
}
