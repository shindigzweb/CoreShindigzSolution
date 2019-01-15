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
        public List<InstructionSheet> InstructionSheets { get; set; }

        public void OnGet()
        {
            Sheet1 = new InstructionSheet("firstfile.pdf");

            _filenames = Directory.GetFiles(InstructionSheet.FolderPath, "*.pdf", SearchOption.TopDirectoryOnly);

            InstructionSheets = new List<InstructionSheet>();

            foreach(var name in _filenames)
            {
                string itemno = Path.GetFileNameWithoutExtension(name);

                if (!String.IsNullOrEmpty(itemno)){
                    InstructionSheet nextSheet = new InstructionSheet() { FileName = itemno };
                    InstructionSheets.Add(nextSheet);
                }
            }
        }
    }
}