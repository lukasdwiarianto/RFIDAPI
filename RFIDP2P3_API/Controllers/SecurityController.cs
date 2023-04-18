using Microsoft.AspNetCore.Mvc;
using RFIDP2P3_API.Models;
using System.Data;
using System.Data.SqlClient;

namespace RFIDP2P3_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly string _configuration;
        private string remarks = string.Empty;

        public SecurityController(IConfiguration configuration)
        {
            _configuration = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpPost]
        public ActionResult Index(MasterUser user)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.password);

            using (SqlConnection conn = new SqlConnection(_configuration))
            using (SqlCommand cmd = new SqlCommand("sp_Submit_UserId", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                cmd.Parameters.Add(new("@PIC_ID", user.PIC_ID));
                cmd.Parameters.Add(new("@Passwords", passwordHash));

                cmd.Parameters["@PIC_ID"].Value = user.PIC_ID;
                cmd.Parameters["@Passwords"].Value = passwordHash;

                conn.Open();
                cmd.ExecuteNonQuery();
                remarks = Convert.ToString(cmd.Parameters["@Remarks"].Value);
                conn.Close();
            }
            if (remarks != "success") return BadRequest(remarks);
            else return Ok(remarks);
        }
    }
}
