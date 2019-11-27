using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PowerBankAdmin.Pages.Tutorial
{
    public class IndexModel : BaseAuthCostumerPage
    {
        public void OnGet()
        {
            IdentifyCostumer();
            ViewData["City"] = "/css/patterns/city_1.png";
        }
    }
}
