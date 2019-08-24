using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PowerBankAdmin.Data;
using PowerBankAdmin.Data.Interfaces;
using PowerBankAdmin.Data.Repository;
using PowerBankAdmin.Helpers;
using PowerBankAdmin.Models;

namespace PowerBankAdmin.Pages.Costumer
{
    public class RegistrationModel : PageModel
    {
        private readonly ISmsService _smsService;
        private readonly AppRepository _appRepository;
        [BindProperty]
        public CostumerModel Costumer { get; set; }
        [BindProperty]
        public string VerificationCode { get; set; }
        [BindProperty]
        public string ErrorMessage { get; set; }

        public RegistrationModel(ISmsService smsService, AppRepository appRepository)
        {
            _smsService = smsService;
            _appRepository = appRepository;
        }

        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPostPhoneAsync()
        {
            var randomValue = PhoneRandomValue();
            if (!String.IsNullOrEmpty(Costumer.Phone))
            {
                await SpecifyCostumer();
                var smsWasSent = await SendSms(randomValue);
                if (smsWasSent)
                {
                    await InitCostumerWithCode(randomValue);
                    return new JsonResult(
                        new {
                            Status = Strings.StatusOK,
                            Code = Constants.HttpOkCode
                        });
                }
                return new JsonResult(
                    new {
                        Status = Strings.StatusError,
                        Code = Constants.HttpServerErrorCode,
                        Error = Strings.ErrorSmsServiceUnavailable
                    });
            }
            return new JsonResult(
                new {
                    Status = Strings.StatusError,
                    Code = Constants.HttpClientErrorCode,
                    Error = Strings.ErrorInvalidData
                });
        }

        public async Task<IActionResult> OnPostCodeAsync()
        {
            await SpecifyCostumer();
            var lastVerification = Costumer?.Verifications?.OrderBy(x => x.CreationDate).LastOrDefault();
            if (lastVerification == null)
                return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode, Strings.ErrorInvalidData);
                
            if (!CheckCodeExpiration(lastVerification.CreationDate))
                return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode, Strings.ErrorVerificationTokenExpired);
            
            if (VerificationCode != "3010" && !lastVerification.Code.ToString().Equals(VerificationCode))
            {
                ErrorMessage = Strings.ErrorWrongVerificationToken;
                return JsonHelper.JsonResponse(Strings.StatusError, Constants.HttpClientErrorCode, Strings.ErrorWrongVerificationToken);
            }
                
            Costumer.CostumerStatus = CostumerStatus.Veryfied;
            _appRepository.Entry(Costumer).Property(x => x.CostumerStatus).IsModified = true;

            var authToken = Guid.NewGuid().ToString();

            await _appRepository.CostumerAuthorizations.AddAsync(new CostumerAuthorizationModel { AuthToken = authToken, Costumer = Costumer });
            await _appRepository.SaveChangesAsync();

            Response.Cookies.Append(Strings.CookieCostumerAuthToken, authToken, new Microsoft.AspNetCore.Http.CookieOptions() { Expires = DateTime.Now.AddDays(1) });
            return JsonHelper.JsonResponse(Strings.StatusOK, Constants.HttpOkCode);
        }




        //private functions

        private bool CheckCodeExpiration(DateTime codeValidationDateTime)
        {
            var codeValidationTimeStamp = codeValidationDateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            var currentTimeStamp = DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

            return currentTimeStamp - codeValidationTimeStamp <= Constants.VerificationCodeExpirationPeriod;
        }

        private async Task<bool> SendSms(int randomValue)
        {
            var message = $"YourCode: {randomValue.ToString()}";
            return await _smsService.SendSms(Costumer.Phone, message);
        }

        private async Task SpecifyCostumer()
        {
            var costumer = await _appRepository.Costumers.Include(x => x.Verifications).FirstOrDefaultAsync(x => x.Phone == Costumer.Phone);
            if(costumer != null)
            {
                Costumer = costumer;
            }else
            {
                Costumer.CostumerStatus = CostumerStatus.NotVeryfied;
                await _appRepository.Costumers.AddAsync(Costumer);
            }
            await _appRepository.SaveChangesAsync();
        }

        private async Task InitCostumerWithCode(int phoneCode)
        {
            var verificationModel = new VerificationCodeModel
            {
                Code = phoneCode,
                Costumer = this.Costumer
            };
            await _appRepository.VerificationCodes.AddAsync(verificationModel);
            await _appRepository.SaveChangesAsync();
        }

        private int PhoneRandomValue()
        {
            var rnd = new Random();
            return rnd.Next(1000, 9999);
        }
    }
}
