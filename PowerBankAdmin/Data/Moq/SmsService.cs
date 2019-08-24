using System;
using System.Threading;
using System.Threading.Tasks;
using PowerBankAdmin.Data.Interfaces;

namespace PowerBankAdmin.Data.Moq
{
    public class SmsService : ISmsService
    {

        public async Task<bool> SendSms(string Phone, string Text)
        {
            return await Task.Run(() =>
            {
                Thread.Sleep(200);
                return true;
            });
        }
    }
}
