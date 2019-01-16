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
        public InstructionSheet Sheet1 { get; set; }
        public string[] _filenames { get; set; }
        public List<InstructionSheet> InstructionSheets { get; set; } = new List<InstructionSheet>();
        public string DirectoryName { get; set; } = InstructionSheet.FolderPath;
        private InstructionSheetRepository _repo;

        public SetupModel(InstructionSheetRepository repo)
        {
            _repo = repo;
        }

        public void OnGet()
        {
            _filenames = Directory.GetFiles(InstructionSheet.FolderPath, "*.pdf", SearchOption.TopDirectoryOnly);

            foreach(var filename in _filenames)
            {
                InstructionSheets.Add( new InstructionSheet() { FileName = Path.GetFileName(filename) } );
            }

            foreach (var instsheet in InstructionSheets)
            {
                _repo.TryUpdateItem(instsheet.FileName);
            }
        }
    }
}