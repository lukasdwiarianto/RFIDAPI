using Microsoft.AspNetCore.Mvc;
using RFIDP2P3_API.Models;
using System.Data;
using System.Data.SqlClient;

namespace RFIDP2P3_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MasterUserGroupController : ControllerBase
    {
        private readonly string _configuration;
        private string remarks = "";

        public MasterUserGroupController(IConfiguration configuration)
        {
            _configuration = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpPost]
        public ActionResult<IEnumerable<MasterUserGroup>> INQ(MasterUserGroup usergroup)
        {
            List<MasterUserGroup> UserGroups = new();

            using (SqlConnection conn = new(_configuration))
            {
                conn.Open();
                SqlCommand cmd = new("exec sp_Inq_M_UserGroup @UserGroup_Id", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new("@UserGroup_Id", usergroup.UserGroup_Id));

                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    UserGroups.Add(new MasterUserGroup
                    {
                        UserGroup_Id = sdr["UserGroup_Id"].ToString(),
                        UserGroup = sdr["UserGroup"].ToString()
                    });
                }
                conn.Close();
            }
            return UserGroups;
        }

        [HttpPost]
        public ActionResult<IEnumerable<MasterUserGroup>> DEL(MasterUserGroup usergroup)
        {
            using (SqlConnection conn = new(_configuration))
            {
                conn.Open();
                SqlCommand cmd = new("exec sp_Delete_M_UserGroup @UserGroup_Id", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new("@UserGroup_Id", usergroup.UserGroup_Id));
                remarks = cmd.ExecuteScalar().ToString();
                conn.Close();
            }
            if (remarks != "success") return BadRequest(remarks);
            else return Ok(remarks);
        }

        [HttpPost]
        public ActionResult<IEnumerable<MasterUserGroup>> INS(MasterUserGroup usergroup)
        {
            using (SqlConnection conn = new(_configuration))
            {
                conn.Open();
                SqlCommand cmd = new("exec sp_Submit_M_UserGroup @UserGroup_Id, @UserGroup, @UserLogin", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new("@UserGroup_Id", usergroup.UserGroup_Id));
                cmd.Parameters.Add(new("@UserGroup", usergroup.UserGroup));
                cmd.Parameters.Add(new("@UserLogin", usergroup.UserLogin));
                remarks = cmd.ExecuteScalar().ToString();
                conn.Close();
            }
            if (remarks != "success") return BadRequest(remarks);
            else return Ok(remarks);
        }
    }
}
