using MassTransit;
using Microsoft.EntityFrameworkCore;
using Syncora.Services.Inventory.Api.Extensions;
using Syncora.Services.Inventory.Api.Infrastructure;
using Syncora.Services.Inventory.Application.Consumers;
using Syncora.Services.Inventory.Application.Interfaces;
using Syncora.Services.Inventory.Domain.AggregatesModel.InventoryAggregate;
using Syncora.Services.Inventory.Infrastructure.Persistence;
using Syncora.Services.Inventory.Infrastructure.Repositories;
using Syncora.Services.Inventory.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<InventoryDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("InventoryDb"),
        sqlOptions =>
        {
            sqlOptions.MigrationsAssembly(typeof(InventoryDbContext).Assembly.FullName);
            sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
        });
});

builder.Services.AddMassTransit(x =>
{
    // Register the Consumer
    x.AddConsumer<OrderCreatedConsumer>();
    x.AddConsumer<OrderCancelledConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        // Automatically create queues for registered consumers
        cfg.ConfigureEndpoints(context);
    });
});


builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IInventoryQueryService, InventoryQueryService>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Syncora.Services.Inventory.Application.InventoryItems.Commands.CreateInventoryItem.CreateInventoryItemCommand).Assembly));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await app.ApplyDatabaseMigrationAsync();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
