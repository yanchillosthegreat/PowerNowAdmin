#pragma checksum "C:\Users\yanmoroz\source\repos\PowerBankAdmin\PowerBankAdmin\Pages\Users\AddUser.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "108bd8261baaae5cb9f1841e723bc2ba0a6f91fc"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Pages_Users_AddUser), @"mvc.1.0.razor-page", @"/Pages/Users/AddUser.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure.RazorPageAttribute(@"/Pages/Users/AddUser.cshtml", typeof(AspNetCore.Pages_Users_AddUser), null)]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"108bd8261baaae5cb9f1841e723bc2ba0a6f91fc", @"/Pages/Users/AddUser.cshtml")]
    public class Pages_Users_AddUser : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(55, 14, true);
            WriteLiteral("\r\n\r\n\r\n    <h1>");
            EndContext();
            BeginContext(70, 16, false);
#line 6 "C:\Users\yanmoroz\source\repos\PowerBankAdmin\PowerBankAdmin\Pages\Users\AddUser.cshtml"
   Write(Model.MyProperty);

#line default
#line hidden
            EndContext();
            BeginContext(86, 7, true);
            WriteLiteral("</h1>\r\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<PowerBankAdmin.Pages.Users.AddUserModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<PowerBankAdmin.Pages.Users.AddUserModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<PowerBankAdmin.Pages.Users.AddUserModel>)PageContext?.ViewData;
        public PowerBankAdmin.Pages.Users.AddUserModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591
