using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PowerBankAdmin.Data.Interfaces;
using PowerBankAdmin.Data.Repository;
using PowerBankAdmin.Models;

namespace PowerBankAdmin.Pages.Take
{
    public class SelectTariffModel : BaseAuthCostumerPage
    {
        private readonly AppRepository _appRepository;
        private readonly IHolderService _holderService;

        [BindProperty]
        public PowerbankSessionModel Session { get; set; }
        [BindProperty]
        public double SessionDuration { get; set; }

        public SelectTariffModel(IHolderService holderService, AppRepository appRepository)
        {
            _appRepository = appRepository;
            _holderService = holderService;
        }

        public async Task OnGetAsync()
        {
            IdentifyCostumer();

            if (IsAuthorized())
            {
                //Session = await _holderService.LastSession(Costumer.Id);
                //if (Session != null && Session.IsActive)
                //{
                //    SessionDuration = (DateTime.Now - Session.Start).TotalSeconds;
                //}


            }

            ViewData["HideFooter"] = true;
        }

        public void TakePowerbank()
        {
            var borrowRequest = new BorrowRequest
            {
                EquipmentSn = "1800008823",
                PackageType = "171",
                Position = "1",
                DeviceType = "8",
                DeviceVersion = "1",
                SessionId = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString()
            };

            WebRequest request = WebRequest.Create("https://dry-wildwood-23355.herokuapp.com/operation");
            request.ContentType = "application/json";
            request.Method = "POST";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string body = JsonConvert.SerializeObject(borrowRequest);
                streamWriter.Write(body);
            }

            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
        }

        public async Task<IActionResult> OnPostAddCardAsync()
        {
            base.IdentifyCostumer();
            var _httpClient = new WebClient();
            var random = new Random();
            var orderNumber = random.Next(15000, 16000);
            var urlString = $"https://3dsec.sberbank.ru/payment/rest/register.do?userName=power-now-api&clientId={Costumer.Id}&password=power-now&orderNumber={orderNumber}&amount=1&returnUrl=https://power-now.ru/acquiring/{Strings.GoToTakePage}"; 
             var responseText = await _httpClient.DownloadStringTaskAsync(new Uri(urlString));

            JsonSerializer serializer = new JsonSerializer();
            RegisterDoResponse response = JsonConvert.DeserializeObject<RegisterDoResponse>(responseText);
            await Costumer.SetOrderId(_appRepository, response.OrderId);
            await Costumer.SetCardStatus(_appRepository, CardsStatus.Progress);
            return Redirect(response.FormUrl);
        }
    }



    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class BaseRequest
    {
        [JsonProperty(PropertyName = "equipmentSn")]
        public string EquipmentSn { get; set; }

        [JsonProperty(PropertyName = "deviceType")]
        public string DeviceType { get; set; }

        [JsonProperty(PropertyName = "deviceVersion")]
        public string DeviceVersion { get; set; }

        [JsonProperty(PropertyName = "sessionId")]
        public string SessionId { get; set; }

        [JsonProperty(PropertyName = "signature")]
        public string Signature { get; set; }

        [JsonProperty(PropertyName = "packageType")]
        public string PackageType { get; set; }
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class BorrowRequest : BaseRequest
    {
        [JsonProperty(PropertyName = "position")]
        public string Position { get; set; }

        [JsonProperty(PropertyName = "powerBankSn")]
        public string PowerBankSn { get; set; }
    }

    public class FooRequest
    {
        [JsonProperty(PropertyName = "ciphertext")]
        public string CipherText { get; set; }

        [JsonProperty(PropertyName = "clientId")]
        public string ClientId { get; set; }
    }
}