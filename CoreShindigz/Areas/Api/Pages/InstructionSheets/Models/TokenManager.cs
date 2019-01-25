using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreShindigz.Areas.Api.Pages.InstructionSheets.Models
{
    public class TokenManager
    {
        public static (bool IsLegit, string ItemNo, string Payload) DecodeToken(string token)
        {
            if (token == null)
                throw new ArgumentException("Missing token value");

            byte[] plainTextBytes = System.Convert.FromBase64String(token);
            string plainText = System.Text.Encoding.UTF8.GetString(plainTextBytes);
            string[] args = plainText.Split('|');
            string schema = args[0];
            bool isLegit = true;
            string itemNo = "";

            switch (schema)
            {
                case "IOZ":
                case "IXX":
                    if (args.Length > 1)
                        itemNo = args[1];
                    else
                        isLegit = false;
                    break;
                default:
                    isLegit = false;
                    break;
            }

            return (IsLegit: isLegit, ItemNo: itemNo, Payload: plainText);
        }

        public static string EncodeToken(params string[] args)
        {
            var plainText = String.Join("|", args);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);            
        }
    }
}
