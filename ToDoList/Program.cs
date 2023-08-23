using Microsoft.EntityFrameworkCore;
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

// Configura��o da string de conex�o.
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Adiciona o DbContext ao container de servi�os.
builder.Services.AddDbContext<TarefaContext>(options =>
    options.UseSqlServer(connectionString));

// Register the services and repositories you created with the connection string.
builder.Services.AddScoped<ITarefaService, TarefaService>();
builder.Services.AddScoped<ITarefaRepository>(provider =>
    new TarefaRepository(provider.GetRequiredService<TarefaContext>(), connectionString));
builder.Services.AddScoped<IErrorRepository>(provider =>
    new ErrorRepository(provider.GetRequiredService<TarefaContext>(), connectionString));
builder.Services.AddScoped<IErrorService, ErrorService>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
