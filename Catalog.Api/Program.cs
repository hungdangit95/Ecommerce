using Catalog.Api.Data;
using Catalog.Api.Model;
using Catalog.Api.Repositories;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<DatabaseSettings>(options =>
{
    options.ConnectionString = configuration.GetSection("DatabaseSettings:ConnectionString").Value;
    options.DatabaseName = configuration.GetSection("DatabaseSettings:DatabaseName").Value;
});

builder.Services.AddScoped<IMongoDbContext, MongoDbContext>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
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
