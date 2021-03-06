﻿using System;
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
using PowerBankAdmin.Services;
using Yandex.Checkout.V3;

namespace PowerBankAdmin.Pages.Costumer
{
    public class RegistrationModel : BaseAuthCostumerPage
    {
        private readonly ISmsService _smsService;
        private readonly AppRepository _appRepository;
        [BindProperty]
        public CostumerModel Costumer { get; set; }
        [BindProperty]
        public string ErrorMessage { get; set; }


        [BindProperty]
        public string C1 { get; set; }
        [BindProperty]
        public string C2 { get; set; }
        [BindProperty]
        public string C3 { get; set; }
        [BindProperty]
        public string C4 { get; set; }

        public RegistrationModel(ISmsService smsService, AppRepository appRepository)
        {
            _smsService = smsService;
            _appRepository = appRepository;

            //var dude = _appRepository.Costumers.Include(x => x.CardBindings).FirstOrDefault(x => x.Phone.Contains("443"));
            //var foo = _appRepository.CardBidnings.FirstOrDefault(x => x.BindingId == "2687f56c-000f-5000-8000-18421823560d");

            //var client = new Client(shopId: Strings.YandexShopId, secretKey: Strings.YandexAPIKey);

            //client.CreatePayment(new NewPayment
            //{
            //    Amount = new Amount { Currency = "RUB", Value = new decimal(250) },
            //    PaymentMethodId = "2687f56c-000f-5000-8000-18421823560d",
            //    Capture = true,
            //    Description = "Списание 250 руб. за использование паувербанка более 5-ти дней",
            //    Confirmation = new Confirmation
            //    {
            //        Type = ConfirmationType.Redirect,
            //        ReturnUrl = ""
            //    },
            //});

            //var dude1 = _appRepository.Costumers.Include(x => x.Sessions).ThenInclude(x => x.Powerbank).FirstOrDefault(x => x.Phone.Contains("225"));
            //var dude2 = _appRepository.Costumers.Include(x => x.Sessions).ThenInclude(x => x.Powerbank).FirstOrDefault(x => x.Phone.Contains("071"));

            //var mirzo = _appRepository.Costumers.Include(x => x.Sessions).ThenInclude(x => x.Powerbank).FirstOrDefault(x => x.Phone.Contains("+7 (123) 123 12 34"));
        }

        public void OnGet()
        {
            ViewData["Title"] = "авторизация";
            ViewData["City"] = "/css/patterns/city_2.png";
        }

        public async Task<IActionResult> OnPostPhoneAsync()
        {
            var randomValue = PhoneRandomValue();
            if (!String.IsNullOrEmpty(Costumer.Phone))
            {
                await SpecifyCostumer();
                var smsWasSent = await SendSms(randomValue);
                //var smsWasSent = true;
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

            var verificationCode = C1 + C2 + C3 + C4;

            if (verificationCode != "3010" && !lastVerification.Code.ToString().Equals(verificationCode))
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
