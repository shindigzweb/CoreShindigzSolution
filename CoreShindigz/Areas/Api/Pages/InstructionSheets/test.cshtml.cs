using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

using CoreShindigz.Areas.Api.Pages.InstructionSheets.v1.Models;

namespace CoreShindigz.Areas.Api.Pages.InstructionSheets
{
    public class testModel : PageModel
    {
        private InstructionSheetRepository _repo;

        public string FolderLocation { get; set; }
        public List<InstructionSheet> InstructionSheets { get; set; }

        public testModel(IConfiguration configuration)
        {
            _repo = new InstructionSheetRepository(configuration);
            FolderLocation = _repo.FolderPath;
        }
        public void OnGet()
        {
            InstructionSheets = _repo.GetFolderListings();

            Page();
        }
    }
}