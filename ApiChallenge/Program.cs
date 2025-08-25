using Application.Interfaces;
using Application.Services;
using Infraestructure.DataBaseContext;
using Infraestructure.ElascticSearch;
using Infraestructure.Kafka;
using Infraestructure.Repositorie;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);
// Configurar Serilog
// Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // Lee el appsettings.json
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(builder.Configuration["Elasticsearch:Url"]))
    {
        AutoRegisterTemplate = true,
        IndexFormat = $"{builder.Configuration["Elasticsearch:DefaultIndex"]}-{DateTime.UtcNow:yyyy-MM}"
    })
    .CreateLogger();

builder.Host.UseSerilog();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppContextSql>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppSqlServer")));


// ------------------ Elasticsearch ------------------
builder.Services.AddSingleton<ElacticService>();

// ------------------ Kafka ------------------
builder.Services.AddSingleton<KafkaService>();

builder.Services.AddScoped(typeof(IRepositorieAbstraction<>), typeof(RespositorieAbstraction<>));
builder.Services.AddScoped<IChallanceService, ChallanceService>();
var app = builder.Build();
app.UseSerilogRequestLogging();  
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
