using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using NetCoreOData.API.Models;



static IEdmModel GetEdmModel()
{
    ODataConventionModelBuilder builder = new();
    builder.EntitySet<Product>("Products");
    builder.EntitySet<Category>("Categories");


    //.../odata/categories(1)/TotalPrice
    builder.EntityType<Category>().Action("TotalPrice").Returns<int>();

    //.../odata/categories/TotalPrice    collection koleksiyon üzerinde çalýþýlacaðýný belirtir
    builder.EntityType<Category>().Collection.Action("TotalPrice1").Returns<int>();

    //.../odata/categories/TotalWithParameters 
    builder.EntityType<Category>().Collection.Action("TotalWithParameters").Returns<int>().Parameter<int>("categoryId");

    var action = builder.EntityType<Category>().Collection.Action("Toplama").Returns<int>();
    action.Parameter<int>("a");
    action.Parameter<int>("b");
    action.Parameter<int>("c");



    builder.EntityType<Category>().Collection.Action("LoginUser").Returns<string>().Parameter<Login>("login");

    builder.EntityType<Category>().Collection.Function("CategoryCount").Returns<int>();


    var function = builder.EntityType<Category>().Collection.Function("Carpma").Returns<int>();
    function.Parameter<int>("a");
    function.Parameter<int>("b");
    function.Parameter<int>("c");


    //UnBound function ve action aynýdýr
    builder.Function("Kdv").Returns<int>();

    return builder.GetEdmModel();

}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>

    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"))

);
builder.Services.AddControllers().AddOData(opt => opt.AddRouteComponents("odata", GetEdmModel()).Filter().Select().Expand().OrderBy().SetMaxTop(null).Count());
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("odata", new() { Title = "NetCoreOData", Version = "odata" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/odata/swagger.json", "NetCoreOData odata"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

