using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace crimes.Pages
{
    public class AboutModel : PageModel
    {
        public string Message { get; set; }
        public string Message1{ get; set; }

        public void OnGet()
        {
            Message = "C# web application for 'Chicago Crimes' designed using ASP.NET and ADO.NET";
        }
    }
}
