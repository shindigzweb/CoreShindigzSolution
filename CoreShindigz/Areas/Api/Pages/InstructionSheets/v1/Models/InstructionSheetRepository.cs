﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Dapper;

namespace CoreShindigz.Areas.Api.Pages.InstructionSheets.v1.Models
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
        public string FolderPath { get; set; } = @"\\southprod2\InstSheets";

        /// <summary>
        /// Constructor initializes 2 connection strings for ASP1 and EcometryDataWarehouse
        /// </summary>
        /// <param name="configuration"></param>
        public InstructionSheetRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connStringASP1 = configuration.GetConnectionString("ASP1");
            _connStringEDW = configuration.GetConnectionString("EcometryDataWarehouse");
        }

        /// <summary>
        /// Updates EDW.Itemmastplus with a filename that matches first word of itemno
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>Int number of rows updated</returns>
        public int TryUpdateItem(string filename)
        {
            var itemno = Path.GetFileNameWithoutExtension(filename);

            if(filename.Length < 26)
            {
                var Sql = "UPDATE ITEMMASTPLUS SET INSTRSHEETFILENAME = '" + filename + "'" +
                          "WHERE LEFT(ITEMNO,CHARINDEX(' ',ITEMNO)) = '" + itemno + "'" +
                          "AND INSTRSHEETFILENAME IS NULL";

                using (SqlConnection conn = new SqlConnection(_connStringEDW))
                {
                    return conn.Execute(Sql);
                }
            }

            return 0;
        }

        /// <summary>
        /// Returns the filename for a given itemno
        /// </summary>
        /// <param name="itemno">The ecometry item number to find a filename for</param>
        /// <returns>String filename</returns>
        public string GetFileNameForItem(string itemno)
        {
            var Sql = $"SELECT INSTRSHEETFILENAME FROM ITEMMASTPLUS WHERE ITEMNO = '{itemno}'";

            using (SqlConnection conn = new SqlConnection(_connStringEDW))
            {
                return conn.QuerySingleOrDefault<string>(Sql);
            }
        }

        /// <summary>
        /// Lists the content of the InstructionSheet Folder Location
        /// </summary>
        /// <returns> a List<InstructionSheet></returns>
        public List<InstructionSheet> GetFolderListings()
        {
            var instructionSheets = new List<InstructionSheet>();

            string[] listings = Directory.GetFiles(FolderPath, "*", SearchOption.TopDirectoryOnly);

            foreach (var filename in listings)
            {
                instructionSheets.Add(new InstructionSheet(Path.GetFileName(filename)));
            }

            return instructionSheets;
        }

        /// <summary>
        /// Reads the file from the network
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>FileContentResult</returns>
        public FileContentResult GetFile(string filename)
        {
            string fileUNC = $@"{FolderPath}\{filename}";

            var bytes = System.IO.File.ReadAllBytes(fileUNC);

            return new FileContentResult(bytes, GetContentType(filename));
        }

        /// <summary>
        /// Utility method for setting mime type on download
        /// </summary>
        /// <param name="path">Any string representing path to a file (can be just the filename itself</param>
        /// <returns>the mime type for the extension</returns>
        public static string GetContentType(string path)
        {
            var types = new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };

            var ext = Path.GetExtension(path).ToLowerInvariant();

            return types[ext];
        }

        public bool VerifyTokenValues(string itemNo, string orderNo, string postalCode)
        {
            string verifiedItemNo;
            string postalCode5 = postalCode.Substring(0, 5);

            string Sql = "SELECT IM.ITEMNO FROM ORDERDETL OD INNER JOIN ORDERHEADER OH ON OD.ORDERNO = OH.ORDERNO " +
                         "INNER JOIN CUSTOMERS CUST ON OH.CUSTEDP = CUST.CUST_EDP " + 
                         "INNER JOIN ITEMMAST IM ON OD.EDPNO = IM.EDPNO " +
                         "WHERE(IM.ITEMNO = @itemNo) AND (OD.ORDERNO = @orderNo) AND (LEFT(CUST.ZIP,5) = @postalCode)";

            using (SqlConnection conn = new SqlConnection(_connStringEDW))
            {
                verifiedItemNo = conn.QuerySingleOrDefault<string>(Sql,new {itemNo, orderNo, @postalCode = postalCode5 });
            }

            return verifiedItemNo != null;
        }
    }
}
