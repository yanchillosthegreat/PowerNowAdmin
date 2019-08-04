using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PowerBankAdmin.Data.Moq;
using PowerBankAdmin.Data.Repository;
using PowerBankAdmin.Models;

namespace PowerBankAdmin.Pages.Auth
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public UserModel User { get; set; }
        public string ErrorMessage { get; set; }

        private readonly AppRepository _appRepository;


        public LoginModel(AppRepository appRepository)
        {
            _appRepository = appRepository;
        }


        public void OnGet()
        {
        }



        public async Task<IActionResult>  OnPostAsync()
        {
            var user = await _appRepository.Users.FirstOrDefaultAsync(x => x.Login == User.Login && x.Password == User.Password);
            if (user == null)
            {
                ErrorMessage = "Login Incorrect";
                return Page();
            }

            var authToken = Guid.NewGuid().ToString();

            await _appRepository.Authorizations.AddAsync(new AuthorizationModel { AuthToken = authToken, User = user });
            await _appRepository.SaveChangesAsync();

            Response.Cookies.Append("authToken", authToken, new Microsoft.AspNetCore.Http.CookieOptions() { Expires = DateTime.Now.AddDays(1) });
            return RedirectToPage("/Index");
        }

    }
}
