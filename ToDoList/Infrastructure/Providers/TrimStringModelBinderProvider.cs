﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using ToDoList.Infrastructure.Binders;

namespace ToDoList.Infrastructure.Providers
{
    public class TrimStringModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(string))
            {
                return new TrimStringModelBinder();
            }

            return null;
        }
    }

}
