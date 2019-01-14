using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CoreShindigz.Areas.Api.Pages.InstructionSheets.Models;

namespace CoreShindigz.Areas.Api.Pages.InstructionSheets
{
    public class SetupModel : PageModel
    {
        public void OnGet()
        {

        }

        public List<InstructionSheet> InstructionSheets { get; set; }
    }
}