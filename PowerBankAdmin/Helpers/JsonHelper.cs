using System;
using Microsoft.AspNetCore.Mvc;

namespace PowerBankAdmin.Helpers
{
    public static class JsonHelper
    {
        public static IActionResult JsonResponse(string status, int code, string message = "nothing to say")
        {
            return new JsonResult(
                    new
                    {
                        Status = status,
                        Code = code,
                        Message = message
                    });
        }
    }
}
