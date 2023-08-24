using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.FIlters.Swagger;
using ToDoList.Repository.Implementations;
using ToDoList.Repository.Interfaces;
using ToDoList.Services.Implementations;
using ToDoList.Services.Interfaces;
using ToDoList.Utils;

var builder = WebApplication.CreateBuilder(args);
    
// Read the ApplicationUrl from configuration (this will get the environment-specific value)
var applicationUrl = builder.Configuration["AppSettings:ApplicationUrl"] ?? "http://localhost:5000";
builder.WebHost.UseUrls(applicationUrl);

// Configures connection string
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add the DbContext to the services container.
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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "ToDoList", Version = "v1" });
    c.SchemaFilter<CustomSchemaFilter>();
});

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
