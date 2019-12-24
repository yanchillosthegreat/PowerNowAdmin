using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PowerBankAdmin.Pages.Info
{
    public class AgreementModel : BaseAuthCostumerPage
    {
        public void OnGet()
        {
            IdentifyCostumer();
            ViewData["Title"] = "œŒÀ‹«Œ¬¿“≈À‹— Œ≈ —Œ√À¿ÿ≈Õ»≈";
            ViewData["HideFooter"] = true;
        }
    }
}