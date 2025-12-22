using Microsoft.AspNetCore.Mvc.ModelBinding;
using Pacovallet.Core.Controller;

namespace Pacovallet.Core.Extensions
{
    public class DateTimeCustomConverterForModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(DateTime))
            {
                return new DateTimeCustomConverterForModelBinder();
            }

            return null;
        }
    }
}
