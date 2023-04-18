using Microsoft.AspNetCore.Mvc;
using RFIDP2P3_API.Models;
using System.Data;
using System.Data.SqlClient;

namespace RFIDP2P3_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MasterGateController : ControllerBase
    {
        private readonly string _configuration;
        private string remarks = "";

        public MasterGateController(IConfiguration configuration)
        {
            _configuration = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpPost]
        public ActionResult<IEnumerable<MasterGate>> INQ()
        {
            List<MasterGate> Gates = new();

            using (SqlConnection conn = new SqlConnection(_configuration))
            using (SqlCommand cmd = new SqlCommand("sp_M_Gate_Sel", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    Gates.Add(new MasterGate
                    {
                        Gate_ID = sdr["Gate_ID"].ToString(),
                        GateGroup = sdr["GateGroup"].ToString(),
                        ShopCode = sdr["ShopCode"].ToString(),
                        Antenna_ID = sdr["Antenna_ID"].ToString()
                    });
                }
                conn.Close();
            }
            return Gates;
        }

        [HttpPost]
        public ActionResult<IEnumerable<MasterGate>> DEL(MasterGate gate)
        {
            using (SqlConnection conn = new(_configuration))
            {
                conn.Open();
                SqlCommand cmd = new("exec sp_Delete_T_Master_Gate @Gate_ID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new("@Gate_ID", gate.Gate_ID));
                remarks = cmd.ExecuteScalar().ToString();
                conn.Close();
            }
            if (remarks != "success") return BadRequest(remarks);
            else return Ok(remarks);
        }

        [HttpPost]
        public ActionResult<IEnumerable<MasterGate>> INS(MasterGate gate)
        {
            using (SqlConnection conn = new(_configuration))
            {
                conn.Open();
                SqlCommand cmd = new("exec sp_Submit_T_Master_Gate @Gate_ID, @GateGroup, @ShopCode, @UserLogin", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new("@Gate_ID", gate.Gate_ID));
                cmd.Parameters.Add(new("@GateGroup", gate.GateGroup));
                cmd.Parameters.Add(new("@ShopCode", gate.ShopCode));
                cmd.Parameters.Add(new("@UserLogin", gate.UserLogin));
                remarks = cmd.ExecuteScalar().ToString();
                conn.Close();
            }
            if (remarks != "success") return BadRequest(remarks);
            else return Ok(remarks);
        }
    }
}
