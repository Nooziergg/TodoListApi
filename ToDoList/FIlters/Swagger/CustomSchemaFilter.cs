using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ToDoList.FIlters.Swagger
{
    public class CustomSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(DateTime))
            {
                schema.Example = new OpenApiString(DateTime.Now.ToString("dd/MM/yyyy"));
            }
        }
    }
}
