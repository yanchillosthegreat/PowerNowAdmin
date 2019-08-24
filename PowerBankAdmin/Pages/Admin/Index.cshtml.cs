using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PowerBankAdmin.Models;

namespace PowerBankAdmin.Pages.Admin
{
    public class IndexModel : BaseAuthedPageModel
    {
        public void OnGet()
        {
            IdentifyUser();
        }

    }
}