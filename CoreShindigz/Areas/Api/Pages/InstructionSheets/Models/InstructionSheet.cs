using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreShindigz.Areas.Api.Pages.InstructionSheets.Models
{
    public class InstructionSheet
    {
        private const string FolderPath = "";
        public string Filename { get; set; }
        public string Ext { get; set; }
        public string UNCpath
        {
            get
            {
                return $@"{FolderPath}/{this.Filename}.{this.Ext}";
            }
        }
    }
}
