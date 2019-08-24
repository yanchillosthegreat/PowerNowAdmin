using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PowerBankAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerBankAdmin.Pages.Admin
{
    public class BaseAuthedPageModel : PageModel
    {
        public new UserModel User { get; set; }


        public void IdentifyUser()
        {
            User = Request.Headers.ContainsKey(Strings.UserObject) ?
                    JsonConvert.DeserializeObject<UserModel>(Request.Headers[Strings.UserObject]) :
                    new UserModel { Login = "Not Authorized" };
        }

        protected IActionResult Logout()
        {
            Response.Cookies.Delete(Strings.CookieAuthToken);
            return RedirectToPage(Strings.UrlAdminLoginPage);
        }
    }
}
