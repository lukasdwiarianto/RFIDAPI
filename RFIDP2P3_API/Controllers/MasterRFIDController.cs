using Microsoft.AspNetCore.Mvc;
using RFIDP2P3_API.Models;
using System.Data;
using System.Data.SqlClient;

namespace RFIDP2P3_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MasterRFIDController : ControllerBase
    {
        private readonly string _configuration;
        private string remarks = "";

        public MasterRFIDController(IConfiguration configuration)
        {
            _configuration = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpPost]
        public ActionResult<IEnumerable<MasterRFID>> INQ()
        {
            List<MasterRFID> RFIDs = new();

            using (SqlConnection conn = new(_configuration))
            {
                conn.Open();
                SqlCommand cmd = new("SELECT * FROM T_Master_RFID ORDER BY RFID_No", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    RFIDs.Add(new MasterRFID
                    {
                        RFID_No = sdr["RFID_No"].ToString(),
                        RFID_Type = sdr["RFID_Type"].ToString(),
                        Supplier = sdr["Supplier"].ToString(),
                        SupplierName = sdr["SupplierName"].ToString()
                    });
                }
                conn.Close();
            }
            return RFIDs;
        }

        [HttpPost]
        public ActionResult<IEnumerable<MasterRFID>> DEL(MasterRFID rfid)
        {
            using (SqlConnection conn = new(_configuration))
            {
                conn.Open();
                SqlCommand cmd = new("exec sp_Delete_T_Master_RFID @RFID_No", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new("@RFID_No", rfid.RFID_No));
                remarks = cmd.ExecuteScalar().ToString();
                conn.Close();
            }
            if (remarks != "success") return BadRequest(remarks);
            else return Ok(remarks);
        }

        [HttpPost]
        public ActionResult<IEnumerable<MasterRFID>> INS(MasterRFID rfid)
        {
            using (SqlConnection conn = new(_configuration))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                SqlCommand cmd = new("exec sp_Submit_T_Master_RFID @RFID_No, @RFID_Type, @Supplier, '', @UserLogin", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@RFID_No", SqlDbType.NVarChar, 50);
                cmd.Parameters.Add(new("@RFID_Type", rfid.RFID_Type));
                cmd.Parameters.Add(new("@Supplier", rfid.Supplier));
                cmd.Parameters.Add(new("@UserLogin", rfid.UserLogin));
                cmd.Transaction = trans;

                string[] rfids = rfid.RFID_No.Split(";");

                foreach (var no in rfids)
                {
                    cmd.Parameters["@RFID_No"].Value = no;

                    remarks = cmd.ExecuteScalar().ToString();
                    if (remarks != "success")
                    {
                        trans.Rollback();
                        conn.Close();
                        return BadRequest(remarks);
                    }
                }
                trans.Commit();
                conn.Close(); 
                return Ok(remarks);
            }
        }
    }
}
