using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PowerBankAdmin.Pages.Info
{
    public class HelpModel : BaseAuthCostumerPage
	{
        public void OnGet()
        {
            IdentifyCostumer();
            ViewData["Title"] = "помощь";
            ViewData["HideFooter"] = true;
        }
    }
}
