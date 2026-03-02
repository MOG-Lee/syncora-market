using StackExchange.Redis;
using Syncora.Services.Basket.Application.Interfaces;
using Syncora.Services.Basket.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);


// Redis must be Singleton
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("Redis");
    if (string.IsNullOrEmpty(connectionString))
        throw new ArgumentNullException("Redis connection string is missing!");

    return ConnectionMultiplexer.Connect(connectionString);
});

builder.Services.AddScoped<IBasketRepository, BasketRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
