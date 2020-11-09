using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PowerBankAdmin.Data.Interfaces;

namespace PowerBankAdmin.Services
{
    public class GeocodeService : IGeocodeService
    {
        public GeocodeService()
        {
        }

        private string _apiKey = "fa11c2ec-7c4a-4330-b2dd-bfad66375b6c";
        private string _baseUrl = "https://geocode-maps.yandex.ru/1.x/?";


        public async Task<IEnumerable<Address>> AutosuggestAddress(string address)
        {
            var data = await GetDataFromYandex(address);
            var objData = JsonConvert.DeserializeObject<YandexResponse>(data,
                new JsonSerializerSettings
                {
                    Error = (sender, args) =>
                    {
                        Console.WriteLine(args.ErrorContext.Error.Message);
                        args.ErrorContext.Handled = true;
                    },
                    Converters = { new IsoDateTimeConverter() }
                }
            );
            var geoObjects = objData?.Response?.GeoObjectCollection?.FeatureMember;
            if(geoObjects == null)
                return null;
            var res = new List<Address>();
            foreach(var geoObject in geoObjects)
            {
                var lat = geoObject.GeoObject?.Point?.Pos?.Split(' ')?[1];
                var lon = geoObject.GeoObject?.Point?.Pos?.Split(' ')?[0]; 

                var a = new Address
                {
                    FormattedAddress = String.Format("{0}, {1}", geoObject?.GeoObject?.Description, geoObject?.GeoObject?.Name),
                    Latitude = lat,
                    Longitude = lon
                };
                res.Add(a);
            }
            return res;
        }

        private async Task<string> GetDataFromYandex(string address)
        {
            var url = $"{_baseUrl}apikey={_apiKey}&format=json&geocode={address}";


            var uri = new Uri(url);

            var request = WebRequest.Create(uri);


            var response = await request.GetResponseAsync();
            string responceString = "";

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                responceString = await reader.ReadToEndAsync();
            }
            return responceString;
        }

        public async Task<(string latitude, string longitude)> Geocode(string address)
        {
            var adresses = await AutosuggestAddress(address);
            return (adresses?.ToList().FirstOrDefault()?.Latitude, adresses?.ToList().FirstOrDefault()?.Longitude);
        }
    }
    public class YandexResponse
    {
        [JsonProperty("response")]
        public Response Response { get; set; }
    }
    public class Response
    {
        [JsonProperty("GeoObjectCollection")]
        public GeoObjectCollection GeoObjectCollection { get; set; }
    }
    public class GeoObjectCollection
    {
        [JsonProperty("metaDataProperty")]
        public MetaDataProperty MetaDataProperty { get; set; }
        [JsonProperty("featureMember")]
        public IEnumerable<Geo> FeatureMember { get; set; }
    }
    public class Geo
    {
        [JsonProperty("GeoObject")]
        public GeoObject GeoObject { get; set; }
    }
    public class GeoObject
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("Point")]
        public Point Point { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
    }

    public class Point
    {
        [JsonProperty("pos")]
        public string Pos { get; set; }
    }

    public class MetaDataProperty
    {
        [JsonProperty("GeocoderResponseMetaData")]
        public GeocoderResponseMetaData GeocoderResponseMetaData { get; set; }
    }

    public class GeocoderResponseMetaData
    {
        public string Request { get; set; }
        public string Results { get; set; }
        public string Found { get; set; }
    }



}
