using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using PowerBankAdmin.Data.Repository;
using PowerBankAdmin.Helpers;
using PowerBankAdmin.Models;
using PowerBankAdmin.Pages.Take;

namespace PowerBankAdmin.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIController : ControllerBase
    {
        private readonly AppRepository _appRepository;
        public APIController(AppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        // GET: api/API
        [HttpGet]
        public async Task<IActionResult> Get(string orderId)
        {
            await Update();
            return Redirect("/costumer");
        }

        public async Task Update()
        {
            var _httpClient = new WebClient();
            var urlString = $"https://3dsec.sberbank.ru/payment/rest/getBindings.do?userName=power-now-api&password=power-now&clientId=778";
            var responseText = _httpClient.DownloadString(new Uri(urlString));

            JsonSerializer serializer = new JsonSerializer();
            GetBindingsResponse response = JsonConvert.DeserializeObject<GetBindingsResponse>(responseText);

            var binding = response.Bindings.FirstOrDefault();
            if (binding != null)
            {
                var Costumer = Request.Headers.ContainsKey(Strings.CostumerObject) ?
                    JsonConvert.DeserializeObject<CostumerModel>(Request.Headers[Strings.CostumerObject]) :
                    new CostumerModel { Name = "Not Authorized" };

                var costumerToEdit = _appRepository.Costumers.FirstOrDefault(x => x.Id == Costumer.Id);
                costumerToEdit.BindId = binding.BindingId;
                costumerToEdit.Card = binding.MaskedPan;
                _appRepository.Entry(Costumer).Property(x => x.BindId).IsModified = true;
                _appRepository.Entry(Costumer).Property(x => x.Card).IsModified = true;
                await _appRepository.SaveChangesAsync();
            }
        }

        // POST: api/API
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/API/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
