using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PowerBankAdmin.Data.Interfaces;
using PowerBankAdmin.Helpers;

namespace PowerBankAdmin.Pages.Equipment
{
    public class IndexModel : PageModel
    {
        IHolderService _holderService;
        public IndexModel(IHolderService holderService)
        {
            _holderService = holderService;
        }

        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPostAsync()
		{
            using (var stream = Request.Body)
            {
                var requestBytes = new byte[stream.Length + 1];
                var requestBody = stream.ReadAsync(requestBytes, 0, (int)stream.Length);
                var requestString = Encoding.UTF8.GetString(requestBytes);
                try
                {
                    var request = JsonConvert.DeserializeObject<ReleasePowerBankRequest>(requestString);
                    if(string.IsNullOrEmpty(request.PowerBankCode) ||
                        string.IsNullOrEmpty(request.HolderCode) ||
                        request.Space < 1)
                    {
                        return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode);
                    }
                    await _holderService.ReleasePowerBank(request.PowerBankCode, request.HolderCode, request.Space);
                }
                catch
                {
                    return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode);
                }
                
            }
            return JsonHelper.JsonResponse(Strings.StatusOK, Constants.HttpOkCode);
        }
    }

    public class ReleasePowerBankRequest
	{
        public string PowerBankCode { get; set; }
        public string HolderCode { get; set; }
        public int Space { get; set; }
    }
}
