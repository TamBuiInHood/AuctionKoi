using KoiAuction.Repository.Entities;
using KoiAuction.Repository.IRepositories;
using KoiAuction.Repository.Repositories;
using KoiAuction.WebApp.Controllers;
using PRN231.AuctionKoi.API.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<Fa24Se1716Prn231G5KoiauctionContext>();
builder.Services.AddScoped<IAutionRepository, AutionRepository>();
builder.Services.AddHttpClient<AuctionsController>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7094/"); 
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
//builder.Services.AddScoped<PaymentController>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
