using BusinessObject;
using DataAccess;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace eStore.Pages.Admin
{
    public class ManageOrdersModel : PageModel
    {
        private readonly OrderRepository _orderRepository;

        public ManageOrdersModel(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public List<Order> Orders { get; set; } = new();
        [BindProperty] public Order NewOrder { get; set; } = new();

        public void OnGet()
        {
           
            
                Orders = _orderRepository.GetAllOrders()
                          .Select(o => new Order
                          {
                              OrderId = o.OrderId,
                              OrderDate = o.OrderDate,
                              RequiredDate = o.RequiredDate,
                              ShippedDate = o.ShippedDate,
                              Freight = o.Freight,
                              MemberId = o.MemberId,
                              Member = new Member { Email = o.Member?.Email } // Chỉ lấy email để tối ưu hóa dữ liệu
                          }).ToList();
            }



        public IActionResult OnPostUpdateOrderDate(int id, Order UpdatedOrder)
        {
            var order = _orderRepository.GetOrderById(id);
            if (order != null)
            {
                order.OrderDate = UpdatedOrder.OrderDate;
                _orderRepository.UpdateOrder(order);
            }
            return RedirectToPage();
        }

        public IActionResult OnPostUpdateRequiredDate(int id, Order UpdatedOrder)
        {
            var order = _orderRepository.GetOrderById(id);
            if (order != null)
            {
                order.RequiredDate = UpdatedOrder.RequiredDate;
                _orderRepository.UpdateOrder(order);
            }
            return RedirectToPage();
        }

        public IActionResult OnPostUpdateShippedDate(int id, Order UpdatedOrder)
        {
            var order = _orderRepository.GetOrderById(id);
            if (order != null)
            {
                order.ShippedDate = UpdatedOrder.ShippedDate;
                _orderRepository.UpdateOrder(order);
            }
            return RedirectToPage();
        }
        public IActionResult OnPost()
        {
            string action = Request.Form["action"];
            int orderId = int.Parse(Request.Form["id"]);

            var order = _orderRepository.GetOrderById(orderId);
            if (order == null) return RedirectToPage();

            switch (action)
            {
                case "UpdateOrderDate":
                    if (DateTime.TryParse(Request.Form["OrderDate"], out DateTime orderDate))
                    {
                        order.OrderDate = orderDate;
                        _orderRepository.UpdateOrder(order);
                    }
                    break;

                case "UpdateRequiredDate":
                    if (DateTime.TryParse(Request.Form["RequiredDate"], out DateTime requiredDate))
                    {
                        order.RequiredDate = requiredDate;
                        _orderRepository.UpdateOrder(order);
                    }
                    break;

                case "UpdateShippedDate":
                    if (DateTime.TryParse(Request.Form["ShippedDate"], out DateTime shippedDate))
                    {
                        order.ShippedDate = shippedDate;
                        _orderRepository.UpdateOrder(order);
                    }
                    break;

                case "UpdateFreight":
                    if (decimal.TryParse(Request.Form["Freight"], out decimal freight))
                    {
                        order.Freight = freight;
                        _orderRepository.UpdateOrder(order);
                    }
                    break;

                case "Delete":
                    _orderRepository.RemoveOrder(orderId);
                    break;
            }

            return RedirectToPage();
        }


    }
}
