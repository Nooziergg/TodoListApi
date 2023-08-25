using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;

namespace ToDoList.Infrastructure.Binders
{
    public class CustomDateBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult == ValueProviderResult.None)
                return Task.CompletedTask;

            var value = valueProviderResult.FirstValue;

            if (string.IsNullOrEmpty(value))
                return Task.CompletedTask;

            if (DateTime.TryParseExact(value, "dd-MM-yyyy", null, DateTimeStyles.None, out var dateValue))
            {
                bindingContext.Result = ModelBindingResult.Success(dateValue);
            }
            else
            {
                bindingContext.ModelState.TryAddModelError(
                    bindingContext.ModelName,
                    "Date should be in format dd-MM-yyyy");

            }

            return Task.CompletedTask;
        }
    }

}
