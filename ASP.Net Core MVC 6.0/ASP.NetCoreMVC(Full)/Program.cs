using ASP.NetCoreMVCEx1.Services.Implement;
using ASP.NetCoreMVCEx1.Services;
// using ASP.NetCoreMVCEx1.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IRookiesService,RookiesService>();
builder.Services.AddSession();
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
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "NashTech/{controller=Rookies}/{action=Index}/{id?}");
// app.UseMiddleware<CustomMiddleware>();
app.Run();
