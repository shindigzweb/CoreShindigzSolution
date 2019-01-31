using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreShindigz.Areas.Api.Pages.InstructionSheets.Models
{
    public class TokenManager
    {
        public static (bool FormatOk, string ItemNo, string OrderNo, string PostalCode, string Schema) DecodeToken(string token)
        {

            bool formatOk = true;
            string schema = "";
            string itemNo = "";
            string orderNo = "";
            string postalCode = "";
            string plainText = "";

            if (token == null)
                throw new ArgumentException("Missing token value");

            try
            {
                byte[] plainTextBytes = System.Convert.FromBase64String(token);
                plainText = System.Text.Encoding.UTF8.GetString(plainTextBytes);
                string[] args = plainText.Split('|');
                schema = args[0];

                switch (schema)
                {
                    case "IOP":
                        if(args.Length > 3)
                        {
                            itemNo = args[1];
                            orderNo = args[2];
                            postalCode = args[3];
                        }
                        else
                            formatOk = false;
                        break;
                    case "IXX":
                        if (args.Length > 1)
                            itemNo = args[1];
                        else
                            formatOk = false;
                        break;
                    default:
                        formatOk = false;
                        break;
                }
                
            }
            catch (FormatException)
            {
                formatOk = false;
            }

            return (FormatOk: formatOk, ItemNo: itemNo, OrderNo: orderNo, PostalCode: postalCode, Schema: schema);
        }

        public static string EncodeToken(params string[] args)
        {
            var plainText = String.Join("|", args);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);

            return System.Convert.ToBase64String(plainTextBytes);            
        }
    }
}
