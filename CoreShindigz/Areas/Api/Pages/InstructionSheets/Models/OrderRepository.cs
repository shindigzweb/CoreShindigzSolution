using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using CoreShindigz.Areas.Api.Pages.InstructionSheets.Models;
using Dapper;

namespace CoreShindigz.Areas.Api.Pages.InstructionSheets.Models
{
    public class OrderRepository
    {
        private string _connStringEDW;

        public OrderRepository(IConfiguration configuration)
        {
            _connStringEDW = configuration.GetConnectionString("EcometryDataWarehouse");
        }

        /// <summary>
        /// Check to see if order number and postalcode are good combination
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="postalCode"></param>
        /// <returns>bool</returns>
        public bool IsRequestLegit(string orderNo, string postalCode)
        {
            var conn = new SqlConnection(_connStringEDW);

            var sql = "SELECT OH.CUSTEDP FROM ORDERHEADER OH INNER JOIN CUSTOMERS CUST ON " +
                "OH.CUSTEDP = CUST.CUST_EDP WHERE OH.ORDERNO = @orderNo AND CUST.ZIP = @postalCode";

            var result = conn.Query(sql, new { orderNo, postalCode });

            return result.Count() > 0;
        }

        public List<InstructionSheetAsset> GetInstructionSheets(string orderNo)
        {
            var conn = new SqlConnection(_connStringEDW);

            var sql =   "SELECT IM.ITEMNO, IM.DESCRIPTION, IMP.INSTRSHEETFILENAME AS FILENAME " +
                        "FROM ORDERHEADER OH INNER JOIN ORDERDETL OD ON " +
                            "OH.ORDERNO = OD.ORDERNO INNER JOIN ITEMMAST IM ON " +
                            "OD.EDPNO = IM.EDPNO INNER JOIN ITEMMASTPLUS IMP ON " +
                            "IM.EDPNO = IMP.EDPNO " +
                         "WHERE(OH.ORDERNO = @orderNo) " +
                         "AND(IMP.INSTRSHEETFILENAME IS NOT NULL)";

            return conn.Query<InstructionSheetAsset>(sql, new { orderNo }).ToList();
        }
    }

    public class InstructionSheetAsset 
    {
        public string ItemNo { get; set; }
        public string Description { get; set; }
        public InstructionSheet InstructionSheet { get; set; }

        public InstructionSheetAsset(string itemNo, string description, string fileName)
        {
            ItemNo = itemNo;
            Description = description;
            InstructionSheet = new InstructionSheet(fileName);
        }
    }



}
