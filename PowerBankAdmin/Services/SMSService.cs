using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PowerBankAdmin.Data.Interfaces;

namespace PowerBankAdmin.Services
{

    public class SMSService : ISmsService
    {
        public async Task<bool> SendSms(string Phone, string Text)
        {
            var sign = "SMS Aero";
            var channel = "DIRECT";

            var url = $"https://gate.smsaero.ru/v2/sms/send?number={Phone}&text={Text}&sign={sign}&channel={channel}";
            var result = await SendRequest(url);
            return result.Success;
        }

        public async Task<bool> SendFreeSms()
        {
            var url = "https://gate.smsaero.ru/v2/sms/testsend?number=79057015196&text=Test&sign=BIZNES&channel=DIRECT";
            var result = await SendRequest(url);
            return result.Success;
        }


        private async Task<SendSMSResponse> SendRequest(string url)
        {
            var uri = new Uri(url);
            var request = WebRequest.Create(uri);
            request.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            request.Headers.Add(HttpRequestHeader.Authorization, "Basic bWlyem9raGFzaGltb3ZtQGdtYWlsLmNvbTpoMFRRUWFDZFRvNUFOanlpQ0ZGZlNXNXdIRXNo");

            var response = await request.GetResponseAsync();
            var responceObject = new SendSMSResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = await reader.ReadToEndAsync();
                responceObject = JsonConvert.DeserializeObject<SendSMSResponse>(responseFromServer);
            }
            return responceObject;
        }
    }





    public class SendSMSResponse
    {
        //public string OrderId { get; set; }
        //public string FormUrl { get; set; }

        public bool Success { get; set; }
        public string Message { get; set; }
        public SendSMSResponseData Data { get; set; }
    }

    public class SendSMSResponseData
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string Number { get; set; }
        public string Text { get; set; }
        public int Status { get; set; }
        public string ExtendStatus { get; set; }
        public string Channel { get; set; }
        public double Cost { get; set; }
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime DateCreate { get; set; }
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime DateSend { get; set; }
    }


    public class MicrosecondEpochConverter : DateTimeConverterBase
    {
        private static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(((DateTime)value - _epoch).TotalMilliseconds + "000");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) { return null; }
            return _epoch.AddMilliseconds((long)reader.Value / 1000d);
        }
    }
}
