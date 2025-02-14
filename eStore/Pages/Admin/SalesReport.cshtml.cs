using DataAccess.DAO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eStore.Pages.Admin
{
    public class SalesReportModel : PageModel
    {
        [BindProperty]
        public DateTime StartDate { get; set; }

        [BindProperty]
        public DateTime EndDate { get; set; }

        public List<SalesReport> SalesData { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }

        public string SalesLabelsJson { get; set; }
        public string SalesDataJson { get; set; }

        public void OnGet()
        {
            StartDate = DateTime.Today.AddDays(-30);
            EndDate = DateTime.Today;
            LoadSalesData();
        }

        public void OnPost()
        {
            // Ensure StartDate is before EndDate
            if (StartDate > EndDate)
            {
                ModelState.AddModelError("StartDate", "Ngày bắt đầu không thể lớn hơn ngày kết thúc.");
                return;
            }

            LoadSalesData();
        }

        private void LoadSalesData()
        {
            SalesData = OrderDAO.GetSalesReport(StartDate, EndDate);

            if (SalesData == null || !SalesData.Any())
            {
                // Handle no data scenario (optional)
                TotalRevenue = 0;
                TotalOrders = 0;
                SalesLabelsJson = "[]";
                SalesDataJson = "[]";
                return;
            }

            // Tổng số đơn hàng & tổng doanh thu
            TotalRevenue = SalesData.Sum(s => s.TotalRevenue);
            TotalOrders = SalesData.Sum(s => s.TotalOrders);

            // Chuyển dữ liệu thành JSON để vẽ biểu đồ
            var salesDataForChart = SalesData.Select(s => new
            {
                Date = s.Date.ToString("dd/MM/yyyy"),
                Revenue = s.TotalRevenue
            }).ToList();

            SalesLabelsJson = JsonConvert.SerializeObject(salesDataForChart.Select(s => s.Date));
            SalesDataJson = JsonConvert.SerializeObject(salesDataForChart.Select(s => s.Revenue));
        }
    }
}
