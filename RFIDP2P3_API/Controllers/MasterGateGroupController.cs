using Microsoft.AspNetCore.Mvc;
using RFIDP2P3_API.Models;
using System.Data;
using System.Data.SqlClient;

namespace RFIDP2P3_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MasterGateGroupController : ControllerBase
    {
        private readonly string _configuration;
        private string remarks = "";

        public MasterGateGroupController(IConfiguration configuration)
        {
            _configuration = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpPost]
        public ActionResult<IEnumerable<MasterGateGroup>> INQ()
        {
            List<MasterGateGroup> GateGroups = new();

            using (SqlConnection conn = new(_configuration))
            {
                conn.Open();
                SqlCommand cmd = new("SELECT * FROM T_Master_Gate_Group ORDER BY GateGroup, ShopCode", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    GateGroups.Add(new MasterGateGroup
                    {
                        GateGroup = sdr["GateGroup"].ToString(),
                        ShopCode = sdr["ShopCode"].ToString()
                    });
                }
                conn.Close();
            }
            return GateGroups;
        }

        [HttpPost]
        public ActionResult<IEnumerable<MasterGateGroup>> DEL(MasterGateGroup gategroup)
        {
            using (SqlConnection conn = new(_configuration))
            {
                conn.Open();
                SqlCommand cmd = new("exec sp_Delete_T_Master_Gate_Group @GateGroup, @ShopCode", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new("@GateGroup", gategroup.GateGroup));
                cmd.Parameters.Add(new("@ShopCode", gategroup.ShopCode));
                remarks = cmd.ExecuteScalar().ToString();
                conn.Close();
            }
            if (remarks != "success") return BadRequest(remarks);
            else return Ok(remarks);
        }

        [HttpPost]
        public ActionResult<IEnumerable<MasterGateGroup>> INS(MasterGateGroup gategroup)
        {
            using (SqlConnection conn = new(_configuration))
            {
                conn.Open();
                SqlCommand cmd = new("exec sp_Submit_T_Master_Gate_Group @GateGroup, @ShopCode", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new("@GateGroup", gategroup.GateGroup));
                cmd.Parameters.Add(new("@ShopCode", gategroup.ShopCode));
                remarks = cmd.ExecuteScalar().ToString();
                conn.Close();
            }
            if (remarks != "success") return BadRequest(remarks);
            else return Ok(remarks);
        }
    }
}
