using Microsoft.AspNetCore.Mvc;
using RFIDP2P3_API.Models;
using System.Data;
using System.Data.SqlClient;

namespace RFIDP2P3_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MasterPrivilegeController : ControllerBase
    {
        private readonly string _configuration;
        private string remarks = "";

        public MasterPrivilegeController(IConfiguration configuration)
        {
            _configuration = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpPost]
        public ActionResult<IEnumerable<Privilege>> INQ(MasterPrivilege privilege)
        {
            List<Privilege> privileges = new();

            using (SqlConnection conn = new(_configuration))
            {
                conn.Open();
                SqlCommand cmd = new("exec sp_Inq_M_UserPreviledge @UserGroup_Id", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new("@UserGroup_Id", privilege.UserGroup_Id));

                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    privileges.Add(new Privilege
                    {
                        Menu_Group = sdr["MenuGroupID"].ToString(),
                        Menu_Id = sdr["MenuId"].ToString(),
                        Menu_Name = sdr["MenuName"].ToString(),
                        checkedbox_read = sdr["checkedbox_read"].ToString(),
                        checkedbox_add = sdr["checkedbox_add"].ToString(),
                        checkedbox_edit = sdr["checkedbox_edit"].ToString(),
                        checkedbox_del = sdr["checkedbox_del"].ToString()
                    });
                }
                conn.Close();
            }
            return privileges;
        }

        [HttpPost]
        public ActionResult<IEnumerable<MasterPrivilege>> INS(MasterPrivilege privileges)
        {
            using (SqlConnection conn = new(_configuration))
            {
                conn.Open();
                SqlCommand cmd = new("exec sp_Delete_M_UserPreviledge @UserGroup_Id", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new("@UserGroup_Id", privileges.UserGroup_Id));

                remarks = cmd.ExecuteScalar().ToString();
                if (remarks == "success")
                {
                    SqlTransaction trans = conn.BeginTransaction();
                    cmd = new("exec sp_Submit_M_UserPreviledge @Menu_Id, @UserGroup_Id, @checkedbox_read, @checkedbox_add, @checkedbox_edit, @checkedbox_del", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Menu_Id", SqlDbType.NVarChar, 25);
                    cmd.Parameters.Add("@UserGroup_Id", SqlDbType.NVarChar, 3);
                    cmd.Parameters.Add("@checkedbox_read", SqlDbType.Char, 1);
                    cmd.Parameters.Add("@checkedbox_add", SqlDbType.Char, 1);
                    cmd.Parameters.Add("@checkedbox_edit", SqlDbType.Char, 1);
                    cmd.Parameters.Add("@checkedbox_del", SqlDbType.Char, 1);
                    cmd.Transaction = trans;
                    foreach (var privilege in privileges.Privileges)
                    {
                        cmd.Parameters["@Menu_Id"].Value = privilege.Menu_Id;
                        cmd.Parameters["@UserGroup_Id"].Value = privileges.UserGroup_Id;
                        cmd.Parameters["@checkedbox_read"].Value = privilege.checkedbox_read;
                        cmd.Parameters["@checkedbox_add"].Value = privilege.checkedbox_add;
                        cmd.Parameters["@checkedbox_edit"].Value = privilege.checkedbox_edit;
                        cmd.Parameters["@checkedbox_del"].Value = privilege.checkedbox_del;
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
                else
                {
                    conn.Close();
                    return BadRequest(remarks);
                }
            }
        }
    }
}
