using System;
namespace PowerBankAdmin
{
    public static class Constants
    {
        //HTML Codes
        public const int HttpOkCode = 200;
        public const int HttpClientErrorCode = 400;
        public const int HttpServerErrorCode = 500;

        //VerificationSettings
        public const int VerificationCodeExpirationPeriod = 5 * 60;
    }
}
