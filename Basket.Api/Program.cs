using Basket.Api.GrpcServices;
using Basket.Api.Repositories;
using Discount.Grpc.Protos;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = configuration.GetValue<string>("CacheSettings:ConnectionString");
});

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>
             (o => o.Address = new Uri(configuration["GrpcSettings:DiscountUrl"]));

builder.Services.AddScoped<DiscountGrpcService>();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddAutoMapper(typeof(Program));

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
