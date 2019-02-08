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
    public class ListModel : PageModel
    {
        public List<InstructionSheetAsset> InstructionSheetAssets { get; set; } = new List<InstructionSheetAsset>();
        public string OrderNo { get; set; } = "";
        public string PostalCode { get; set; } = "";
        public string Message { get; set; } = "";
        private OrderRepository _orderRepo;

        public ListModel(IConfiguration configuration)
        {
            _orderRepo = new OrderRepository(configuration);
        }

        public void OnGet(string token = "")
        {
            // token = OPX|ORDERNO|POSTALCODE
            string orderNo;
            string postalCode;
            var tokenArgs = token.Split('|');

            if (tokenArgs.Length < 3)
            {
                Message = "Incorrect data.";
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
                Message = "Invalid data.";
                Page();
                return;
            }

            // do we have instruction sheet assets for this order
            this.OrderNo = orderNo;
            this.PostalCode = postalCode;
            this.InstructionSheetAssets = _orderRepo.GetInstructionSheets(orderNo);

            if(InstructionSheetAssets.Count < 1)
            {
                Message = "No instruction sheets listed for any item on this order";
            }

            Page();
        }
    }
}