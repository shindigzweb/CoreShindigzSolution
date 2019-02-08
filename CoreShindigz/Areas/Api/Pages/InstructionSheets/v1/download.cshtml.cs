using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using CoreShindigz.Areas.Api.Pages.InstructionSheets.v1.Models;

namespace CoreShindigz.Areas.Api.Pages.InstructionSheets.v1
{
    public class downloadModel : PageModel
    {
        private InstructionSheetRepository _repo;

        public downloadModel(IConfiguration configuration)
        {
            _repo = new InstructionSheetRepository(configuration);
        }
        
        public IActionResult OnGet(string token)
        {
            string filename = null;

            if (token == null)
                return NotFound();

            //convert the token back to values
            var result = TokenManager.DecodeToken(token);

            if (result.FormatOk)
            {
                if (result.Schema == "IXX")
                {
                    filename = _repo.GetFileNameForItem(result.ItemNo);
                }
                else
                {
                    // Does not really belong in REPO but form now to keep things simple
                    bool isVerified = _repo.VerifyTokenValues(result.ItemNo, result.OrderNo, result.PostalCode);

                    if (isVerified)
                    {
                        filename = _repo.GetFileNameForItem(result.ItemNo);

                    }
                }

                if (filename != null)
                    return _repo.GetFile(filename);
            }

            return NotFound();
        }
    }
}