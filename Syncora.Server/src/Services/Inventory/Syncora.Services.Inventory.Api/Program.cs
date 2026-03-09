using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Syncora.Services.Inventory.Api.Extensions;
using Syncora.Services.Inventory.Api.Infrastructure;
using Syncora.Services.Inventory.Application.Consumers;
using Syncora.Services.Inventory.Application.Interfaces;
using Syncora.Services.Inventory.Domain.AggregatesModel.InventoryAggregate;
using Syncora.Services.Inventory.Infrastructure.Persistence;
using Syncora.Services.Inventory.Infrastructure.Repositories;
using Syncora.Services.Inventory.Infrastructure.Services;
using System.Text;


//Setup Serilog before the builder
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting Inventory API...");

    var builder = WebApplication.CreateBuilder(args);

    //Full serilog Setup: Read config from appsettings.json
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

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

        x.AddEntityFrameworkOutbox<InventoryDbContext>(o =>
        {
            o.QueryDelay = TimeSpan.FromSeconds(10);
            o.UseSqlServer();
            o.UseBusOutbox(); //This only covers IPublishEndpoint (outside consumers)
        });

        // This tells MassTransit to enable the Outbox on all Receive Endpoints (Consumers)
        x.AddConfigureEndpointsCallback((context, name, cfg) =>
        {
            cfg.UseEntityFrameworkOutbox<InventoryDbContext>(context);
        });

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

    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(secretKey)
        };
    });

    builder.Services.AddSwaggerGen(c =>
    {
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme.",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                },
                Array.Empty<string>()
            }
        });
    });

    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAngularClient", policy =>
        {
            policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    });

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        await app.ApplyDatabaseMigrationAsync();
    }

    app.UseExceptionHandler();

    app.UseHttpsRedirection();

    app.UseCors("AllowAngularClient");

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}