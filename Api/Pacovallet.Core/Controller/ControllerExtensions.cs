using Microsoft.AspNetCore.Mvc; 

namespace Pacovallet.Core.Controller
{
    public static class ControllerExtensions
    {
        public static IActionResult ToActionResult<T>(
        this ControllerBase controller,
        ServiceResponse<T> response)
        {
            if (response.Success)
                return controller.Ok(response.Data);

            return controller.StatusCode(
                (int)response.StatusCode,
                new
                {
                    message = response.Message,
                    status = (int)response.StatusCode
                });
        }

    }
}
