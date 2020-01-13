using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PowerBankAdmin.Data.Repository;
using PowerBankAdmin.Helpers;
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

        public bool IsAuthorized()
        {
            return Request.Headers.ContainsKey(Strings.CostumerObject);
        }

        public string GetName()
        {
            return string.IsNullOrEmpty(Costumer.Name) ? Costumer.Phone : Costumer.Name;
        }


        public IActionResult OnPostLogOut()
        {
            Response.Cookies.Delete(Strings.CookieCostumerAuthToken);
            return JsonHelper.JsonResponse(Strings.StatusOK, Constants.HttpOkCode);
        }
    }
}
