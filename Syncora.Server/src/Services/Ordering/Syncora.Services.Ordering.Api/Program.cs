using Microsoft.EntityFrameworkCore;
using Syncora.Services.Ordering.Api.Extensions;
using Syncora.Services.Ordering.Api.Infrastructure;
using Syncora.Services.Ordering.Application.Interfaces;
using Syncora.Services.Ordering.Domain.AggregatesModel.OrderAggregate;
using Syncora.Services.Ordering.Infrastructure.Persistence;
using Syncora.Services.Ordering.Infrastructure.Repositories;
using Syncora.Services.Ordering.Infrastructure.Services;

// set up kestrel, appsettings.json, logging, environment variables ...
var builder = WebApplication.CreateBuilder(args);

// add context as a Scoped
builder.Services.AddDbContext<OrderingDbContext>(options =>
{
    // set EF to use SQL Server, and get the connection string from appsettings.json
    options.UseSqlServer(builder.Configuration.GetConnectionString("OrderingDb"),
        // retry if DB goes offline especially in Azure
        sqlOptions =>
        {
            // Resiliency (Polly) - retry up to 5 times - each time never waits more than 30 seconds (ex: try1 : 1s - try2: 3s - try3: 8s ...)
            sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
            // Look for migration files in the Infrastructure
            sqlOptions.MigrationsAssembly(typeof(OrderingDbContext).Assembly.FullName);
        });
});

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderQueryService, OrderQueryService>();

//Scan Application layer and find and register all MediatR handlers
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Syncora.Services.Ordering.Application.Orders.Commands.CreateOrder.CreateOrderCommand).Assembly));

// enables the parsing of the controllers like [ApiController]
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//register the global exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
// adds support for returning ProblemDetails in case of exceptions
builder.Services.AddProblemDetails();

//freezes the configuration, we can not add any more services
var app = builder.Build();

// if ASPNETCORE_ENVIRONMENT variable is set to "Development"
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await app.ApplyDatabaseMigrationAsync();
}

//custom global exception handler, will catch all unhandled exceptions
app.UseExceptionHandler();

//Security best practice. to change to HTTPS in production, and redirect HTTP to HTTPS
app.UseHttpsRedirection();

//checking what you can do  *** app.UseAuthentication() (checking who you are)
app.UseAuthorization();

// Map the controllers to endpoints, request /api/orders, go to OrdersController
app.MapControllers();

// Opens the socket, starts listening on the port, and block the thread until the app is closed
app.Run();
