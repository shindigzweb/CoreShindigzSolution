using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreShindigz.Areas.Api.Pages.InstructionSheets.Models
{
    public class InstructionSheet
    {
        public static string FolderPath = @"\\southprod2\InstSheets";
        public string FileName { get; set; }
        public string Ext { get; set; }
        public string UNCpath
        {
            get
            {
                return $@"{FolderPath}\{this.FileName}";
            }
        }

        public InstructionSheet() : this("")
        {
            
        }

        public InstructionSheet(string filename = "")
        {
            FileName = filename;
            Ext = "pdf";
        }
    }
}
