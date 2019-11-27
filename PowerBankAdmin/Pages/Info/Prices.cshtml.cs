﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PowerBankAdmin.Pages.Info
{
    public class PricesModel : BaseAuthCostumerPage
    {
        public void OnGet()
        {
            IdentifyCostumer();
            ViewData["Title"] = "тарифы";
        }
    }
}
