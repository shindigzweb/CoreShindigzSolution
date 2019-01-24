using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreShindigz.Areas.Api.Pages.InstructionSheets.Models
{
    public class TokenManager
    {
        public (bool IsLegit, string Schema, string Payload) DecodeToken(string token)
        {
            bool isLegit = false;
            string schema = "xxx";
            string payload = "";



            return (IsLegit: isLegit, Schema: schema, Payload: payload);
        }

        public string EncodeToken(string[] args)
        {
            var plainText = String.Join("|", args);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);            }
        }
    }
}
