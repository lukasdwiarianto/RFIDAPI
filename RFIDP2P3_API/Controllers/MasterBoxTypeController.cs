using Microsoft.AspNetCore.Mvc;
using RFIDP2P3_API.Models;
using System.Data;
using System.Data.SqlClient;

namespace RFIDP2P3_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MasterBoxTypeController : ControllerBase
    {
        private readonly string _configuration;
        private string remarks = "";

        public MasterBoxTypeController(IConfiguration configuration)
        {
            _configuration = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpPost]
        public ActionResult<IEnumerable<MasterBoxType>> INQ()
        {
            List<MasterBoxType> BoxTypes = new();

            using (SqlConnection conn = new(_configuration))
            {
                conn.Open();
                SqlCommand cmd = new("SELECT * FROM T_Master_RFID_Type ORDER BY RFID_Type", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    BoxTypes.Add(new MasterBoxType
                    {
                        RFID_Type = sdr["RFID_Type"].ToString()
                    });
                }
                conn.Close();
            }
            return BoxTypes;
        }

        [HttpPost]
        public ActionResult<IEnumerable<MasterBoxType>> DEL(MasterBoxType boxtype)
        {
            using (SqlConnection conn = new(_configuration))
            {
                conn.Open();
                SqlCommand cmd = new("exec sp_Delete_T_Master_RFID_Type @RFID_Type", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new("@RFID_Type", boxtype.RFID_Type));
                remarks = cmd.ExecuteScalar().ToString();
                conn.Close();
            }
            if (remarks != "success") return BadRequest(remarks);
            else return Ok(remarks);
        }

        [HttpPost]
        public ActionResult<IEnumerable<MasterBoxType>> INS(MasterBoxType boxtype)
        {
            using (SqlConnection conn = new(_configuration))
            {
                conn.Open();
                SqlCommand cmd = new("exec sp_Submit_T_Master_RFID_Type @RFID_Type", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new("@RFID_Type", boxtype.RFID_Type));
                remarks = cmd.ExecuteScalar().ToString();
                conn.Close();
            }
            if (remarks != "success") return BadRequest(remarks);
            else return Ok(remarks);
        }
    }
}
