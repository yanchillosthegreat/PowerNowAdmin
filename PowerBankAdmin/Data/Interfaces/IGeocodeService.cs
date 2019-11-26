using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PowerBankAdmin.Models;


namespace PowerBankAdmin.Data.Interfaces
{
    public interface IGeocodeService
    {
        Task<(string latitude, string longitude)> Geocode(string address);
        Task<IEnumerable<Address>> AutosuggestAddress(string address);
    }
}
