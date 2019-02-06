using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using CoreShindigz.Areas.Api.Pages.InstructionSheets.Models;

/// <summary>
/// All of Instruction sheet logic is handled by this page.
/// No repo's are injected. in stead we initialize them in the constructor
/// </summary>
namespace CoreShindigz.Areas.Api.Pages.InstructionSheets
{
    public enum PageModes { dev, listfororder, notfound }
        

    public class IndexModel : PageModel
    {
        private InstructionSheetRepository _repo;
        private OrderRepository _orderRepo;
        
        public string FolderLocation;
        public List<InstructionSheet> InstructionSheets;
        public List<InstructionSheetAsset> InstructionSheetAssets;
        public PageModes PageMode;
        public string OrderNo = "";
        public string PostalCode = "";

        public IndexModel(IConfiguration configuration)
        {
            _repo = new InstructionSheetRepository(configuration);
            _orderRepo = new OrderRepository(configuration);
            PageMode = PageModes.dev;
        }
        
     
        public void OnGet()
        {
            this.InstructionSheets = _repo.GetFolderListings();
            this.FolderLocation = _repo.FolderPath;

            Page();
        }

        public IActionResult OnGetDownload(string token)
        {
            string filename = null;

            if (token == null)
                return NotFound();

            //convert the token back to values
            var result = TokenManager.DecodeToken(token);

            if (result.FormatOk)
            {
                if(result.Schema == "IXX")
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

        public void OnGetList(string token)
        {
            // token = OPX|ORDERNO|POSTALCODE
            string orderNo;
            string postalCode;
            var tokenArgs = token.Split('|');

            if (tokenArgs.Length < 3)
            {
                PageMode = PageModes.notfound;
                Page();
                return;
            }
            else
            {
                orderNo = tokenArgs[1];
                postalCode = tokenArgs[2];
            }

            // is orderno and postalcode combination correct
            if (!_orderRepo.IsRequestLegit(orderNo, postalCode))
            {
                PageMode = PageModes.notfound;
                Page();
                return;
            }

            // do we have instruction sheet assets for this order
            this.OrderNo = orderNo;
            this.PostalCode = postalCode;
            this.InstructionSheetAssets = _orderRepo.GetInstructionSheets(orderNo);

            if(this.InstructionSheetAssets.Count == 0)
            {
                this.PageMode = PageModes.notfound;
                Page();
                return;
            }

            this.PageMode = PageModes.listfororder;
            Page();
        }

        
    }
}