using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PowerBankAdmin.Data.Repository;
using PowerBankAdmin.Helpers;
using PowerBankAdmin.Models;

namespace PowerBankAdmin.Pages.Acquiring
{
    public class IndexModel : PageModel
    {
        private AppRepository _appRepository;
        public IndexModel(AppRepository appRepository)
        {
            _appRepository = appRepository;
        }
        public async Task<IActionResult> OnGetAsync(string from, string orderId)
        {
            if (string.IsNullOrEmpty(orderId)) return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode, "Wrong OrderId");
            var costumer = await CostumerModel.GetCostumerByOrderId(_appRepository, orderId);
            if (costumer == null) return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode, "Wrong OrderId");

            var uri = Strings.SberPostBindings + $"?userName={Strings.SberApiLogin}&password={Strings.SberApiPassword}&clientId={costumer.Id}";
            var request = WebRequest.Create(uri);
            request.Method = "POST";

            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var resultString = streamReader.ReadToEnd();
                var result = JsonConvert.DeserializeObject<AcquaringResponse>(resultString);
                if(result.ErrorCode == "0")
                {
                    if (result.Bindings != null && result.Bindings.Count() > 0)
                    {
                        await costumer.ClearBindings(_appRepository);
                        foreach(var binding in result.Bindings)
                        {
                            await costumer.AddBinding(_appRepository, new CardBindingModel
                            {
                                BindingId = binding.BindingId,
                                ExpiryDate = binding.ExpiryDate,
                                MaskedPan = binding.MaskedPan
                            });
                        }
                    }
                }
            }
            await costumer.SetCardStatus(_appRepository, CardsStatus.Ok);
            return Redirect(from == Strings.GoToClientPage ? "/costumer" : from == Strings.GoToTakePage ? "/take/selecttariff" : "/Index" );
        }
        
        class AcquaringResponse
        {
            public string ErrorCode { get; set; }
            public string ErrorMessage { get; set; }
            public IEnumerable<BindingResponse> Bindings { get; set; }
        }

        class BindingResponse
        {
            public string BindingId { get; set; }
            public string MaskedPan { get; set; }
            public string ExpiryDate { get; set; }
        }

    }
}