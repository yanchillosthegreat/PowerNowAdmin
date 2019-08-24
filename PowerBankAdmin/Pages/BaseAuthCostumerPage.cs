using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PowerBankAdmin.Models;

namespace PowerBankAdmin.Pages
{
    public class BaseAuthCostumerPage : PageModel
    {
        [BindProperty]
        public CostumerModel Costumer { get; set; }

        public void IdentifyCostumer()
        {
            Costumer = Request.Headers.ContainsKey(Strings.CostumerObject) ?
                    JsonConvert.DeserializeObject<CostumerModel>(Request.Headers[Strings.CostumerObject]) :
                    new CostumerModel { Name = "Not Authorized" };
        }
    }
}
