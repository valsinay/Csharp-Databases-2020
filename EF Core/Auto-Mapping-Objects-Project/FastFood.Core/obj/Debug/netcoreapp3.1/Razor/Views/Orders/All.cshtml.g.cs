#pragma checksum "D:\Github Repos\C# Databases\EF Core\Auto-Mapping-Objects-Project\FastFood.Core\Views\Orders\All.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "28927857594ce44997b7392064474979771c8f91"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Orders_All), @"mvc.1.0.view", @"/Views/Orders/All.cshtml")]
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
#nullable restore
#line 1 "D:\Github Repos\C# Databases\EF Core\Auto-Mapping-Objects-Project\FastFood.Core\Views\_ViewImports.cshtml"
using FastFood.Core;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"28927857594ce44997b7392064474979771c8f91", @"/Views/Orders/All.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4ed879bff0478396c899ea94a6589fe8b9c42e19", @"/Views/_ViewImports.cshtml")]
    public class Views_Orders_All : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IList<FastFood.Core.ViewModels.Orders.OrderAllViewModel>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "D:\Github Repos\C# Databases\EF Core\Auto-Mapping-Objects-Project\FastFood.Core\Views\Orders\All.cshtml"
  
    ViewData["Title"] = "All Orders";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"<h1 class=""text-center"">All Orders</h1>
<hr class=""hr-2"" />
<table class=""table mx-auto"">
    <thead>
        <tr class=""row"">
            <th class=""col-md-1"">#</th>
            <th class=""col-md-2"">OrderId</th>
            <th class=""col-md-2"">Customer</th>
            <th class=""col-md-2"">Employee</th>
            <th class=""col-md-2"">DateTime</th>
        </tr>
    </thead>
    <tbody>
");
#nullable restore
#line 19 "D:\Github Repos\C# Databases\EF Core\Auto-Mapping-Objects-Project\FastFood.Core\Views\Orders\All.cshtml"
         for(var i = 0; i < Model.Count(); i++)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <tr class=\"row\">\r\n            <th class=\"col-md-1\">");
#nullable restore
#line 22 "D:\Github Repos\C# Databases\EF Core\Auto-Mapping-Objects-Project\FastFood.Core\Views\Orders\All.cshtml"
                            Write(i);

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n            <td class=\"col-md-2\">");
#nullable restore
#line 23 "D:\Github Repos\C# Databases\EF Core\Auto-Mapping-Objects-Project\FastFood.Core\Views\Orders\All.cshtml"
                            Write(Model[i].OrderId);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n            <td class=\"col-md-2\">");
#nullable restore
#line 24 "D:\Github Repos\C# Databases\EF Core\Auto-Mapping-Objects-Project\FastFood.Core\Views\Orders\All.cshtml"
                            Write(Model[i].Customer);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n            <td class=\"col-md-2\">");
#nullable restore
#line 25 "D:\Github Repos\C# Databases\EF Core\Auto-Mapping-Objects-Project\FastFood.Core\Views\Orders\All.cshtml"
                            Write(Model[i].Employee);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n            <td class=\"col-md-2\">");
#nullable restore
#line 26 "D:\Github Repos\C# Databases\EF Core\Auto-Mapping-Objects-Project\FastFood.Core\Views\Orders\All.cshtml"
                            Write(Model[i].DateTime);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n        </tr>\r\n");
#nullable restore
#line 28 "D:\Github Repos\C# Databases\EF Core\Auto-Mapping-Objects-Project\FastFood.Core\Views\Orders\All.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </tbody>\r\n</table>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IList<FastFood.Core.ViewModels.Orders.OrderAllViewModel>> Html { get; private set; }
    }
}
#pragma warning restore 1591
