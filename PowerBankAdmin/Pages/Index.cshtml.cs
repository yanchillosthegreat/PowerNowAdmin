using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PowerBankAdmin.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public int Name { get; set; }

        [BindProperty]
        public List<int> Ints { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            Name = 3;
            Ints = new List<int> { 1, 2, 3 };
            return Page();
        }

    }
}