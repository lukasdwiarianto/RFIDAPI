using Microsoft.AspNetCore.Mvc;
using RFIDP2P3_API.Models;
using System.Data;
using System.Data.SqlClient;

namespace RFIDP2P3_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MasterAreaController : ControllerBase
    {
        private readonly string _configuration;
        private string remarks = "";

        public MasterAreaController(IConfiguration configuration)
        {
            _configuration = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public ActionResult<IEnumerable<MasterArea>> INQ()
        {
            List<MasterArea> Areas = new();

            using (SqlConnection conn = new(_configuration))
            {
                conn.Open();
                SqlCommand cmd = new("SELECT * FROM T_Master_Area ORDER BY Area_Code", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    Areas.Add(new MasterArea
                    {
                        Area_Code = sdr["Area_Code"].ToString(),
                        Area_Name = sdr["Area_Name"].ToString()
                    });
                }
                conn.Close();
            }
            return Areas;
        }

        [HttpPost]
        public ActionResult<IEnumerable<MasterArea>> DEL(MasterArea area)
        {
            using (SqlConnection conn = new(_configuration))
            {
                conn.Open();
                SqlCommand cmd = new("exec sp_Delete_T_Master_Area @Area_Code", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new("@Area_Code", area.Area_Code));
                remarks = cmd.ExecuteScalar().ToString();
                conn.Close();
            }
            if (remarks != "success") return BadRequest(remarks);
            else return Ok(remarks);
        }

        [HttpPost]
        public ActionResult<IEnumerable<MasterArea>> INS(MasterArea area)
        {
            using (SqlConnection conn = new(_configuration))
            {
                conn.Open();
                SqlCommand cmd = new("exec sp_Submit_T_Master_Area @Area_Code, @Area_Name, @UserLogin", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new("@Area_Code", area.Area_Code));
                cmd.Parameters.Add(new("@Area_Name", area.Area_Name));
                cmd.Parameters.Add(new("@UserLogin", area.UserLogin));
                remarks = cmd.ExecuteScalar().ToString();
                conn.Close();
            }
            if (remarks != "success") return BadRequest(remarks);
            else return Ok(remarks);
        }
    }
}
