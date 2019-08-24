using System;
namespace PowerBankAdmin
{
    public static class Strings
    {
        public const string CookieAuthToken = "authToken";
        public const string CookieCostumerAuthToken = "costumerAuthToken";
        public const string UserObject = "user";
        public const string CostumerObject = "costumer";

        //URL's
        public const string UrlAdminLoginPage = "/admin/auth/login";
        public const string UrlAdminIndexPage = "/admin/index";
        public const string UrlCostumerRegistrationPage = "/costumer/registration";

        //HTML Statuses
        public const string StatusOK = "OK";
        public const string StatusError = "ERROR";

        //Errors
        public const string ErrorSmsServiceUnavailable = "Смс сервис недоступен. Попробуйте еще раз через какое-то время";
        public const string ErrorInvalidData = "Введены не верные данные";
        public const string ErrorVerificationTokenExpired = "Ваш проверочный код протух, попробуйте еще раз";
        public const string ErrorWrongVerificationToken = "Не верный проверочный код, попробуйте еще раз";

    }
}
