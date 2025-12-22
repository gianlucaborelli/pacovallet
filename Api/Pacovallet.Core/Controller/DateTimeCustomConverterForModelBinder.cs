using Microsoft.AspNetCore.Mvc.ModelBinding;
using Pacovallet.Core.Extensions;

namespace Pacovallet.Core.Controller
{
    public class DateTimeCustomConverterForModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

            var value = valueProviderResult.FirstValue;

            // Insira aqui sua lógica de formatação de data e hora.
            if (DateTime.TryParse(value, out DateTime result))
            {
                if (result.Kind == DateTimeKind.Unspecified)
                {
                    result = result.TransformToUtc(-3);
                }
                else if (result.Kind == DateTimeKind.Local)
                {
                    result = result.ToUniversalTime();
                }

                bindingContext.Result = ModelBindingResult.Success(result);
            }
            else
            {
                bindingContext.Result = ModelBindingResult.Failed();
            }

            return Task.CompletedTask;
        }
    }
}
