using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace CoreShindigz.Areas.Api.Pages.InstructionSheets.Models
{
        /// <summary>
        /// The Instruction Sheet Repository
        /// Handles all the communication from code to file system and database
        /// </summary>
    public class InstructionSheetRepository
    {
        private IConfiguration _configuration;
        private string _connStringASP1;
        private string _connStringEDW;

        /// <summary>
        /// Constructor initializes 2 connection strings for ASP1 and EcometryDataWarehouse
        /// </summary>
        /// <param name="configuration"></param>
        public InstructionSheetRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connStringASP1 = configuration.GetConnectionString("ASP1");
            _connStringEDW  = configuration.GetConnectionString("EcometryDataWarehouse");
        }

        /// <summary>
        /// Updates EDW.Itemmastplus with a filename that matches first word of itemno
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>Int number of rows updated</returns>
        public int TryUpdateItem(string filename)
        {
            var itemno = Path.GetFileNameWithoutExtension(filename);

            var Sql = "UPDATE ITEMMASTPLUS SET INSTRSHEETFILENAME = '" + filename + "'" +
                      "WHERE LEFT(ITEMNO,CHARINDEX(' ',ITEMNO)) = '" + itemno + "'" +
                      "AND INSTRSHEETFILENAME IS NULL";

            using (SqlConnection conn = new SqlConnection(_connStringEDW))
            {
                return conn.Execute(Sql);
            }
        }

        /// <summary>
        /// Returns the filename for a given itemno
        /// </summary>
        /// <param name="itemno"></param>
        /// <returns>String filename</returns>
        public string GetFileNameForItem(string itemno)
        {
            var Sql = $"SELECT INSTRSHEETFILENAME FROM ITEMMASTPLUS WHERE ITEMNO = {itemno}";

            using(SqlConnection conn = new SqlConnection(_connStringEDW))
            {
                return conn.QuerySingle<string>(Sql);
            }
        }

    }
}
