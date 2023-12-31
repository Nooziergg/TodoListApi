using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ToDoList.Data;
using ToDoList.Infrastructure.Binders;
using ToDoList.Infrastructure.FIlters.Swagger;
using ToDoList.Infrastructure.Providers;
using ToDoList.Repository.Implementations;
using ToDoList.Repository.Interfaces;
using ToDoList.Services.Implementations;
using ToDoList.Services.Interfaces;
using ToDoList.Utils;

var builder = WebApplication.CreateBuilder(args);

// Read the ApplicationUrl from configuration (this will get the environment-specific value)
var applicationUrl = builder.Configuration["AppSettings:ApplicationUrl"] ?? "http://localhost:5000";
builder.WebHost.UseUrls(applicationUrl);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (connectionString.StartsWith("EnvironmentVariable:"))
{
    var envVarName = connectionString.Split(":")[1];
    connectionString = Environment.GetEnvironmentVariable(envVarName);
}

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

// Configure CORS to allow all origins, headers, and methods
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddControllers(options =>
{
    options.ModelBinderProviders.Insert(0, new TrimStringModelBinderProvider());
    options.ModelBinderProviders.Insert(0, new CustomDateBinderProvider());
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "ToDoList", Version = "v1" });
    c.SchemaFilter<CustomSchemaFilter>();
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
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

// Use CORS policy
app.UseCors("AllowAll");

app.UseAuthorization();
app.MapControllers();
app.Run();
