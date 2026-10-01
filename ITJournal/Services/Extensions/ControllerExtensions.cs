using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ITJournal.Services.Extensions
{
    public static class ControllerExtensions
    {
        public static int? GetUserId(this ControllerBase controller)
        {
            return controller.User.Identity?.IsAuthenticated == true
                ? int.Parse(controller.User.FindFirstValue(ClaimTypes.NameIdentifier)!)
                : null;
        }
    }
}
