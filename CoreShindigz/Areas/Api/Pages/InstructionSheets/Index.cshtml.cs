﻿using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CoreShindigz.Areas.Api.Pages.InstructionSheets.Models;

namespace CoreShindigz.Areas.Api.Pages.InstructionSheets
{
    public class IndexModel : PageModel
    {
        
        public IndexModel(InstructionSheetRepository repo)
        {
            _repo = repo;
        }
        private InstructionSheetRepository _repo;
        public string FolderLocation; 
        public List<InstructionSheet> InstructionSheets;
     
        public void OnGet()
        {
            this.InstructionSheets = _repo.GetFolderListings();
            this.FolderLocation = _repo.FolderPath;

            Page();
        }

        //public async Task<IActionResult> OnGetDownload(string itemno)
        //{
        //    if (itemno == null)
        //        return Content("itemno not defined");

        //    var memory = new MemoryStream();
        //    var path = $@"{InstructionSheet.FolderPath}\{itemno}.pdf";

        //    using(var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
        //    {
        //        await stream.CopyToAsync(memory);
        //    }
        //    memory.Position = 0;

        //    return File(memory, InstructionSheet.GetContentType(path), $"Instructions for {itemno}");
        //}

        public IActionResult OnGetDownload(string token)
        {
            if (token == null)
                return Content("itemno not defined");

            string filename = _repo.GetFileNameForItem(token);

            return _repo.GetFile(filename);
        }


    }
}