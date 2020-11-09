using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PowerBankAdmin.Data.Repository;
using PowerBankAdmin.Models;
using Microsoft.EntityFrameworkCore;
using PowerBankAdmin.Helpers;
using Yandex.Checkout.V3;

namespace PowerBankAdmin.Pages.Admin.ManualPayments
{
    public class IndexModel : BaseAuthedPageModel
    {
        private AppRepository _appRepository;

        [BindProperty]
        public IEnumerable<CostumerModel> Costumers { get; set; }

        public IndexModel(AppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        public async Task OnGetAsync()
        {
            IdentifyUser();
            Costumers = await _appRepository.Costumers.Include(x => x.CardBindings).ToListAsync();
        }

        public async Task<IActionResult> OnPostTakeMoneyAsync(string bindingId, int? amount)
        {
            ProceedPayment(bindingId, amount ?? 0);

            return JsonHelper.JsonResponse(Strings.StatusOK, Constants.HttpOkCode);
        }

        private void ProceedPayment(string bindingId, int amount)
        {
            var client = new Client(shopId: Strings.YandexShopId, secretKey: Strings.YandexAPIKey);

            client.CreatePayment(new NewPayment
            {
                Amount = new Amount { Currency = "RUB", Value = new decimal(amount) },
                PaymentMethodId = bindingId,
                Capture = true,
                Description = $"Ручное списание администратором",
                Confirmation = new Confirmation
                {
                    Type = ConfirmationType.Redirect,
                    ReturnUrl = ""
                },
            });
        }
    }
}