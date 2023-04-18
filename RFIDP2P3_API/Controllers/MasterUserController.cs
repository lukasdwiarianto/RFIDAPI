using Microsoft.AspNetCore.Mvc;
using RFIDP2P3_API.Models;
using System.Data;
using System.Data.SqlClient;

namespace RFIDP2P3_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MasterUserController : ControllerBase
    {
        private readonly string _configuration;
        private string remarks = "";

        public MasterUserController(IConfiguration configuration)
        {
            _configuration = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpPost]
        public ActionResult<IEnumerable<MasterUser>> INQ(MasterUser user)
        {
            List<MasterUser> users = new();

            using (SqlConnection conn = new(_configuration))
            {
                conn.Open();
                SqlCommand cmd = new("exec sp_Inq_M_UserSetup @UserLogin", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new("@UserLogin", user.UserLogin));

                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    users.Add(new MasterUser
                    {
                        PIC_ID = sdr["UserID"].ToString(),
                        PIC_Name = sdr["UserName"].ToString(),
                        UserGroup_Id = sdr["UserGroupId"].ToString(),
                        UserGroup = sdr["UserGroup"].ToString(),
                    });
                }
                conn.Close();
            }
            return users;
        }

        [HttpPost]
        public ActionResult<IEnumerable<MasterUser>> DEL(MasterUser user)
        {
            using (SqlConnection conn = new(_configuration))
            {
                conn.Open();
                SqlCommand cmd = new("exec sp_Delete_M_UserSetup @PIC_ID, @UserLogin", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new("@PIC_ID", user.PIC_ID));
                cmd.Parameters.Add(new("@UserLogin", user.UserLogin));
                remarks = cmd.ExecuteScalar().ToString();
                conn.Close();
            }
            if (remarks != "success") return BadRequest(remarks);
            else return Ok(remarks);
        }

        [HttpPost]
        public ActionResult<IEnumerable<MasterUser>> INS(MasterUser user)
        {
            string passwordHash = "";
            if(user.password != "") passwordHash = BCrypt.Net.BCrypt.HashPassword(user.password);
            
            using (SqlConnection conn = new(_configuration))
            {
                conn.Open();
                SqlCommand cmd = new("exec sp_Submit_M_UserSetup @PIC_ID, @PIC_Name, @Passwords, @UserGroup_ID, @UserLogin", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new("@PIC_ID", user.PIC_ID));
                cmd.Parameters.Add(new("@PIC_Name", user.PIC_Name));
                cmd.Parameters.Add(new("@Passwords", passwordHash));
                cmd.Parameters.Add(new("@UserGroup_ID", user.UserGroup_Id));
                cmd.Parameters.Add(new("@UserLogin", user.UserLogin));
                remarks = cmd.ExecuteScalar().ToString();
                conn.Close();
            }
            if (remarks != "success") return BadRequest(remarks);
            else return Ok(remarks);
        }
    }
}
