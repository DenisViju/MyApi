using Microsoft.EntityFrameworkCore;
using MyApi.Data;
using MyApi.Middleware;
using MyApi.Repository;
using MyApi.Services;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddScoped<IMesajService, MesajService>();  
builder.Services.AddScoped<IProdusService, ProdusService>();

builder.Services.AddControllers();
builder.Services.AddProblemDetails();

//Add repositories to the container
builder.Services.AddScoped<IProdusRepository, ProdusRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

//inregistrare AplicatieDbContext cu SQLite
builder.Services.AddDbContext<AplicatieDbContext>(options =>
    options.UseSqlite("Data Source=produse.db"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//inregistrare exceptionHandlingMiddleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();






