using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PowerBankAdmin.Helpers;

namespace PowerBankAdmin.Pages.Costumer
{
    public class AddCardModel : BaseAuthCostumerPage
	{
        [BindProperty]
        public string CardNumber { get; set; }
        [BindProperty]
        public string Birthdate { get; set; }
        [BindProperty]
        public string CVV { get; set; }
        [BindProperty]
        public string FullName { get; set; }

        public void OnGet()
        {
			IdentifyCostumer();
		}

        public async Task<IActionResult> OnPostCardAsync()
        {
            return JsonHelper.JsonResponse(Strings.StatusOK, Constants.HttpOkCode);
        }
    }
}
