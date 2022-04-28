using ASP.NetCoreMVC_EX1_.Services.Implement;
using ASP.NetCoreMVC_EX1_.Services;
using ASP.NetCoreMVC_EX1_.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IRookiesService,RookiesService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

// app.UseMiddleware<CustomMiddleware>();
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
    pattern: "NashTech/{controller=Rookies}/{action=Index}/{id?}");

app.Run();
