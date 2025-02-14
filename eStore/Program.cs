using BusinessObject;
using DataAccess;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// ------------------------------
// Đăng ký các dịch vụ (Services)
// ------------------------------
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.AddDbContext<eStoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EStoreDB")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddMudServices();
builder.Services.AddScoped<DataAccess.DAO.MemberDAO>();
builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped< OrderRepository>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// Cấu hình Session: lưu phiên làm việc 2 giờ, cookie bắt buộc và chỉ dùng cho HTTP
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Cấu hình Razor Pages với AutoValidateAntiforgeryToken
builder.Services.AddRazorPages()
    .AddRazorPagesOptions(options =>
    {
        options.Conventions.ConfigureFilter(new AutoValidateAntiforgeryTokenAttribute());
    });

// ------------------------------
// Xây dựng ứng dụng (Build the app)
// ------------------------------
var app = builder.Build();

// ------------------------------
// Cấu hình Pipeline của HTTP request
// ------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Chuyển hướng HTTP sang HTTPS
app.UseHttpsRedirection();

// Cho phép truy cập các file tĩnh (css, js, hình ảnh, ...)
app.UseStaticFiles();

// Định tuyến: phải được gọi trước các middleware khác liên quan đến request
app.UseRouting();

// Kích hoạt Session (đặt sau UseRouting và trước UseAuthorization)
app.UseSession();

// Áp dụng chính sách CORS nếu cần (nếu bạn cần giao tiếp cross-origin)
app.UseCors("AllowAll");

// Áp dụng Authorization (nếu có)
app.UseAuthorization();

// Map các endpoint cho Razor Pages và Controllers
app.MapRazorPages();


app.Run();
