using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ToDoList.Infrastructure.Binders
{
    public class TrimStringModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueProviderResult != ValueProviderResult.None)
            {
                var valueAsString = valueProviderResult.FirstValue?.Trim();
                bindingContext.Result = ModelBindingResult.Success(valueAsString);
            }
            return Task.CompletedTask;
        }
    }

}
