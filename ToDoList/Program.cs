using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using ToDoList.Data;
using ToDoList.Repository.Implementations;
using ToDoList.Repository.Interfaces;
using ToDoList.Services.Implementations;
using ToDoList.Services.Interfaces;
using ToDoList.Utils;

var builder = WebApplication.CreateBuilder(args);

// Read the ApplicationUrl from configuration (this will get the environment-specific value)
var applicationUrl = builder.Configuration["AppSettings:ApplicationUrl"] ?? "http://localhost:5000";
builder.WebHost.UseUrls(applicationUrl);

// Configuração da string de conexão.
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Adiciona o DbContext ao container de serviços.
builder.Services.AddDbContext<TarefaContext>(options =>
    options.UseSqlServer(connectionString));

// Adiciona Dapper para injeção de dependência.
builder.Services.AddTransient<IDbConnection>(b =>
    new SqlConnection(connectionString));

// Registra os serviços e repositórios que você criou.
builder.Services.AddScoped<ITarefaService, TarefaService>();
builder.Services.AddScoped<ITarefaRepository, TarefaRepository>();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
