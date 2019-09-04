using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PowerBankAdmin.Pages.Admin.Holders
{
    public class IndexModel : BaseAuthedPageModel
    {
        public void OnGet()
        {
            IdentifyUser();
        }
    }
}
