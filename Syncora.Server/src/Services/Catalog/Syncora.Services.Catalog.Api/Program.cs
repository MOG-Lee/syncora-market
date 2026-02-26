using Syncora.Services.Catalog.Api.Infrastructure;
using Syncora.Services.Catalog.Application.Interfaces;
using Syncora.Services.Catalog.Infrastructure.Data;
using Syncora.Services.Catalog.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

MongoDbConfiguration.Configure();

builder.Services.Configure<CatalogDatabaseSettings>(
    builder.Configuration.GetSection("CatalogDatabaseSettings"));

// Mongo Client is recommended to be Singleton
builder.Services.AddSingleton<ICatalogContext, CatalogContext>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//register the global exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
// adds support for returning ProblemDetails in case of exceptions
builder.Services.AddProblemDetails();

var app = builder.Build();

app.Services.GetRequiredService<ICatalogContext>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//custom global exception handler, will catch all unhandled exceptions
app.UseExceptionHandler();

app.UseHttpsRedirection();

//TODO**** for serving static files like images, css, js etc. from wwwroot folder ****
//app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
