﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PowerBankAdmin.Data.Interfaces;
using PowerBankAdmin.Data.Repository;
using PowerBankAdmin.Helpers;
using PowerBankAdmin.Services;
using Yandex.Checkout.V3;

namespace PowerBankAdmin.Pages.Equipment
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class IndexModel : PageModel
    {
        IAcquiringService _acquiringService;
        IHolderService _holderService;
        private readonly AppRepository _appRepository;

        public IndexModel(IAcquiringService acquiringService, IHolderService holderService, AppRepository appRepository)
        {
            _acquiringService = acquiringService;
            _holderService = holderService;
            _appRepository = appRepository;
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

        public async Task<IActionResult> OnPostNotify()
        {
            using (var stream = new StreamReader(Request.Body))
            {
                var body = stream.ReadToEnd();
                var notify = JsonConvert.DeserializeObject<EquipmentNotify>(body, new MyDateTimeConverter());

                switch (notify.PackageType) {
                    case "175":
                        var powerbanksNotifies = notify.Data.PowerbankList;

                        await _holderService.UpdatePowerbanksInfo(powerbanksNotifies);

                        foreach (var powerbanksNotify in powerbanksNotifies)
                        {
                            var sessions = _appRepository.PowerbankSessions.Include(x => x.Powerbank).Include(x => x.RentModel).Include(x => x.Costumer).Where(x => x.IsActive);
                            var session = sessions.FirstOrDefault(x => x.Powerbank.Code == powerbanksNotify.PowerBankSn);
                            if (session == null)
                            {
                                continue;
                            }

                            var position = int.Parse(powerbanksNotify.Position);

                            await _holderService.ReleasePowerBank(powerbanksNotify.PowerBankSn, powerbanksNotify.EquipmentSn, position);
                            _acquiringService.ProceedPayment(session);
                        }

                        break;
                    default: break;
                }
            }

            return JsonHelper.JsonResponse(Strings.StatusOK, Constants.HttpOkCode);
        }
    }

    public class MyDateTimeConverter : Newtonsoft.Json.JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var t = (long)reader.Value;
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(t);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    public class EquipmentNotify
    {
        public string Code { get; set; }

        public EquipmentNotifyData Data { get; set; }

        public string PackageType { get; set; }

        public string SessionId { get; set; }

        public DateTime Timestamp { get; set; }
    }

    public class EquipmentNotifyData
    {
        public DateTime DevicePostTime { get; set; }

        public int CanBorrow { get; set; }

        public string SignalStrength { get; set; }

        public string Temperature { get; set; }

        public DateTime AliGenerateTime { get; set; }

        public int CanRefund { get; set; }

        public DateTime Time { get; set; }

        [JsonProperty(PropertyName = "d_version")]
        public string DVersion { get; set; }

        public string EquipmentSn { get; set; }

        [JsonProperty(PropertyName = "s_version")]
        public string SVersion { get; set; }

        public List<EquipmentNotifyPowerbank> PowerbankList { get; set; }
    }

    public class EquipmentNotifyPowerbank
    {
        public string PowerbankTemperature { get; set; }

        public string PowerBankSn { get; set; }

        public string PowerbankVoltage { get; set; }

        public string ElectricityType { get; set; }

        public string Electricity { get; set; }

        public string Position { get; set; }

        public string PowerbankCurrent { get; set; }

        [JsonProperty(PropertyName = "d_version")]
        public string DVersion { get; set; }

        public string EquipmentSn { get; set; }

        [JsonProperty(PropertyName = "s_version")]
        public string SVersion { get; set; }

        public string Status { get; set; }
    }

    public class ReleasePowerBankRequest
	{
        public string PowerBankCode { get; set; }
        public string HolderCode { get; set; }
        public int Space { get; set; }
    }
}
