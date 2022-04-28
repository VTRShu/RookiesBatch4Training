using Microsoft.EntityFrameworkCore;
using EFCore_Ex2.Repositories.EFContext;
using EFCore_Ex2.Services;
using EFCore_Ex2.Services.Implements;
using AutoMapper;
using EFCore_Ex2.Mappings;

var builder = WebApplication.CreateBuilder(args);
var mapperConfig = new MapperConfiguration(mc =>
     {  
         mc.AllowNullCollections = true;
         mc.AllowNullDestinationValues = true;
         mc.AddProfile(new MappingProfile());
     });
IMapper mapper = mapperConfig.CreateMapper();
// Add services to the container.
builder.Services.AddSingleton(mapper);
builder.Services.AddDbContext<ProductStoreDBContext>(options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));
builder.Services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ICategoryService,CategoryService>();
builder.Services.AddTransient<IProductService,ProductService>();
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
