using LifeCycle.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;

namespace LifeCycle.ModelBinderProviders
{
    public class CustomerBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (context.Metadata.ModelType == typeof(Contact))
            {
                return new BinderTypeModelBinder(typeof(BindedContact));
            }
            return null;
        }   
    }
}
