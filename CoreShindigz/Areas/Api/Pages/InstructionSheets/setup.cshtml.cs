using System;
using System.IO;
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
        public List<InstructionSheet> InstructionSheets { get; set; } = new List<InstructionSheet>();
        public string DirectoryName { get; set; }
        private InstructionSheetRepository _repo;

        public SetupModel(InstructionSheetRepository repo)
        {
            _repo = repo;
        }

        public void OnGet()
        {
            
            InstructionSheets = _repo.GetFolderListings();
            DirectoryName = _repo.FolderPath;

            foreach (var instsheet in InstructionSheets)
            {
                _repo.TryUpdateItem(instsheet.FileName);
            }
        }
    }
}