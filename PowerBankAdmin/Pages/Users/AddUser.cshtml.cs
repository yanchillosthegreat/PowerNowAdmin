using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PowerBankAdmin.Pages.Users
{
    public class AddUserModel : PageModel
    {

        [BindProperty]
        public string MyProperty { get; set; }
        public void OnGet()
        {
            MyProperty = "Herr";
        }
    }
}