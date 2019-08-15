using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PowerBankAdmin.Models;

namespace PowerBankAdmin.Pages
{
    public class IndexModel : BaseAuthedPageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            if(Request.Headers.ContainsKey("user"))
            {
                User = JsonConvert.DeserializeObject<UserModel>(Request.Headers["user"]);
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            Response.Cookies.Append("authToken", string.Empty, new Microsoft.AspNetCore.Http.CookieOptions() { Expires = DateTime.Now.AddSeconds(-1) });
            return RedirectToPage("/Auth/Login");
        }
    }
}