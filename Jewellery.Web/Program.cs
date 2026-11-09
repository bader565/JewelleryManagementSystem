
using Jewellery.Application.Interfaces.Repositories;
using Jewellery.Application.Interfaces.Services;
using Jewellery.Application.Interfaces.Services.Cart;
using Jewellery.Application.Interfaces.Services.Documents;
using Jewellery.Infrastructure.Services.Document;
using Jewellery.Application.Interfaces.Services.Orders;
using Jewellery.Infrastructure.Persistence;
using Jewellery.Infrastructure.Repositories;
using Jewellery.Infrastructure.Repositories.Cart;
using Jewellery.Infrastructure.Repositories.Orders;
using Jewellery.Infrastructure.Services;
using Jewellery.Infrastructure.Services.Cart;
using Jewellery.Infrastructure.Services.Documents;
using Jewellery.Infrastructure.Services.Orders;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Database Configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<
    ICertificateParserService,
    CertificateParserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOcrService, OcrService>();
builder.Services.AddScoped<IInvoiceParserService, InvoiceParserService>();
builder.Services.AddScoped<IAppraisalParserService, AppraisalParserService>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICertificateParserService, CertificateParserService>();
builder.Services.AddScoped<ICertificateService, CertificateService>();
builder.Services.AddScoped<IAppraisalReportService, AppraisalReportService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<ICertificateRepository, CertificateRepository>();

builder.Services.AddScoped<ICertificateQueryService, CertificateQueryService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();


app.UseSession();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
