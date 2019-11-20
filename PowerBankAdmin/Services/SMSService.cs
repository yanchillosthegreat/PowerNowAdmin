using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PowerBankAdmin.Services
{
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

    public class SMSService
    {
        private static SMSService _instance;
        private static object syncRoot = new object();

        private HttpClient _httpClient;

        public SMSService()
        {
            _httpClient = new HttpClient();
        }

        public static SMSService getInstance()
        {
            if (_instance == null)
            {
                lock (syncRoot)
                {
                    if (_instance == null)
                        _instance = new SMSService();
                }
            }

            return _instance;
        }

        public void SendSMS(string code, string phoneNumber)
        {
            var login = "mirzokhashimovm@gmail.com";
            var apiKey = "h0TQQaCdTo5ANjyiCFFfSW5wHEsh";
            var sign = "SMS Aero";
            var channel = "DIRECT";

            var urlString = $"https://{login}:{apiKey}@gate.smsaero.ru/v2/sms/send?number={phoneNumber}&text={code}&sign={sign}&channel={channel}";
            //var response = _httpClient.DownloadString(urlString);

            //JsonSerializer serializer = new JsonSerializer();
            //RegisterDoResponse response = JsonConvert.DeserializeObject<RegisterDoResponse>(responseText);
        }

        public async Task<bool> SendSMSTest()
        {
            var login = "mirzokhashimovm@gmail.com";
            var apiKey = "h0TQQaCdTo5ANjyiCFFfSW5wHEsh";
            var sign = "BIZNES";
            var channel = "DIRECT";

            //var urlString = $"https://{login}:{apiKey}@gate.smsaero.ru/v2/sms/testsend?number=79057015196&text=Test&sign={sign}&channel={channel}";
            var urlString = "https://mirzokhashimovm%40gmail.com%3Ah0TQQaCdTo5ANjyiCFFfSW5wHEsh%40gate.smsaero.ru/v2/sms/testsend?number=79057015196&text=Test&sign=BIZNES&channel=DIRECT";
            //var responseString = _httpClient.DownloadString(urlString);


            var baseUri = new Uri("https://mirzokhashimovm%40gmail.com%3Ah0TQQaCdTo5ANjyiCFFfSW5wHEsh%40gate.smsaero.ru");
            var endpoint = new Uri("/v2/sms/testsend?number=79057015196&text=Test&sign=BIZNES&channel=DIRECT");

                var responseString = await _httpClient.GetAsync(new Uri(baseUri, endpoint));

            //var response = JsonConvert.DeserializeObject<SendSMSResponse>(responseString);

            return responseString.IsSuccessStatusCode;
        }
    }
}
