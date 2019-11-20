using System;
using System.Threading.Tasks;

namespace PowerBankAdmin.Data.Interfaces
{
    public interface ISmsService
    {
        Task<bool> SendSms(string Phone, string Text);
        Task<bool> SendFreeSms();

    }
}
