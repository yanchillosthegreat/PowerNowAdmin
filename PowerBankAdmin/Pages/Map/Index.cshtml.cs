using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PowerBankAdmin.Data.Repository;
using PowerBankAdmin.Models;

namespace PowerBankAdmin.Pages.Map
{
    public class IndexModel : BaseAuthCostumerPage
    {
        public IEnumerable<HolderModel> Holders { get; set; }
        private AppRepository _appRepository;
        public IndexModel(AppRepository appRepository)
        {
            _appRepository = appRepository;
        }
        public async Task OnGetAsync() 
        {
            IdentifyCostumer();
            Holders = await _appRepository.Holders.ToListAsync();
            ViewData["Title"] = "КАРТА С ПАУЕР БАНКАМИ";
            ViewData["City"] = "/css/patterns/city_3.png";
        }
    }
}
