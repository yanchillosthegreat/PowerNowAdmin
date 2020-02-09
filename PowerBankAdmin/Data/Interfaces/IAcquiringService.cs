using PowerBankAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerBankAdmin.Data.Interfaces
{
    public interface IAcquiringService
    {
        void ProceedPayment(PowerbankSessionModel session);
    }
}
