using Microsoft.AspNetCore.Mvc;
using RFIDP2P3_API.Models;
using System.Data;
using System.Data.SqlClient;

namespace RFIDP2P3_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UploadProductionController : ControllerBase
    {
        private readonly string _configuration;
        private string remarks = "";

        public UploadProductionController(IConfiguration configuration)
        {
            _configuration = configuration.GetConnectionString("DefaultConnection");
        }
        // wajib 
        [HttpPost]
        public ActionResult<IEnumerable<UploadProduction>> INQ()
        {
            List<UploadProduction> production = new();

            using (SqlConnection conn = new(_configuration))
            {
                conn.Open();
                SqlCommand cmd = new("exec sp_Inq_M_UserPreviledge '1'", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    production.Add(new UploadProduction
                    {
                        Production_Order_No = sdr["Menuid"].ToString(),
                        Material_Number = sdr["MenuGroupID"].ToString(),
                        Material_Name = sdr["MenuName"].ToString(),
                        Material_Desc = sdr["checkedbox_read"].ToString(),
                        Posting_Date = sdr["checkedbox_read"].ToString(),
                        TCODE = sdr["checkedbox_read"].ToString(),
                        MOVEMENT = sdr["checkedbox_read"].ToString(),
                        NoPro = sdr["checkedbox_read"].ToString(),
                        TO = sdr["checkedbox_read"].ToString(),
                        Material_Qty_Prod = sdr["checkedbox_add"].ToString(),
                        Entry_Date = sdr["checkedbox_edit"].ToString(),
                        Entry_User = sdr["checkedbox_del"].ToString()
                    });
                }
                conn.Close();
            }
            return production;
        }


    }
}
