using AutoMapper;
using EFCore_Ex.Mappings;
using EFCore_Ex.Repositories.EFContext;
using EFCore_Ex.Services;
using EFCore_Ex.Services.Implement;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
var mapperConfig = new MapperConfiguration(mc =>
     {
         mc.AddProfile(new MappingProfile());
     });

IMapper mapper = mapperConfig.CreateMapper();
// Add services to the container.
builder.Services.AddSingleton(mapper);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<RookieDBContext>(options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));
builder.Services.AddTransient<IRookieService,RookieService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
