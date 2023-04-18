using Microsoft.AspNetCore.Mvc;
using RFIDP2P3_API.Models;
using System.Data.SqlClient;
using System.Data;

namespace RFIDP2P3_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly string _configuration;
        private string remarks = "";

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpPost]
        public ActionResult<IEnumerable<MasterUser>> Index(string Userlogin, string UserPwd)
        {
            using (SqlConnection conn = new SqlConnection(_configuration))
            using (SqlCommand cmd = new SqlCommand("sp_UserLogin_Sel", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new("@UserId", Userlogin));

                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();

                string Pwd = "";

                while (sdr.Read())
                {
                    Pwd = sdr["Passwords"].ToString();
                }
                
                if (sdr.FieldCount == 0)
                {
                    conn.Close();
                    return BadRequest("User not found/not active");
                }
                else if (!BCrypt.Net.BCrypt.Verify(UserPwd, Pwd))
                {
                    //cmd = new("EXEC sp_Submit_T_User_Login @PIC_ID, '1'", conn);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add(new("@PIC_ID", login.PIC_ID));
                    //cmd.ExecuteNonQuery();
                    conn.Close();
                    return BadRequest("Incorrect login/password");
                }
                else
                {
                    //cmd = new("EXEC sp_Submit_T_User_Login @PIC_ID, '0'", conn);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add(new("@PIC_ID", login.PIC_ID));
                    //cmd.ExecuteNonQuery();

                    List<Privilege> privileges = new();
                    using (SqlCommand cmd1 = new SqlCommand("sp_UserLogin_Sel", conn))
                    {
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.Add(new("@PIC_ID", Userlogin));
                        SqlDataReader sdr1 = cmd.ExecuteReader();
                        while (sdr1.Read())
                        {
                            privileges.Add(new Privilege
                            {
                                Menu_Id = sdr1["MenuName"].ToString(),
                                checkedbox_read = sdr1["AllowAccess"].ToString(),
                                checkedbox_add = sdr1["AllowSubmit"].ToString(),
                                checkedbox_edit = sdr1["AllowUpdate"].ToString(),
                                checkedbox_del = sdr1["AllowDelete"].ToString()
                            });
                        }
                        sdr1.Close();
                    }
                   
                    List<User> userLogin = new();
                    using (SqlCommand cmd2 = new SqlCommand("sp_UserLogin_Sel", conn))
                    {
                        cmd2.Parameters.Add(new("@PIC_ID", Userlogin));
                        sdr = cmd2.ExecuteReader();
                        while (sdr.Read())
                        {
                            userLogin.Add(new User
                            {
                                PIC_ID = sdr["UserID"].ToString(),
                                PIC_Name = sdr["PIC_Name"].ToString(),
                                UserGroup_Id = sdr["UserGroupID"].ToString(),
                                Privileges = privileges
                            });
                        }
                        conn.Close();
                    }

                    return Ok(userLogin);
                }
            }
        }

        //[HttpPost]
        //public ActionResult Logout(MasterUser logout)
        //{
        //    using (SqlConnection conn = new(_configuration))
        //    {
        //        conn.Open();
        //        SqlCommand cmd = new("EXEC sp_Submit_T_User_Logout @PIC_ID", conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add(new("@PIC_ID", logout.PIC_ID));
        //        remarks = cmd.ExecuteScalar().ToString();
        //        conn.Close();
        //    }
        //    if (remarks != "success") return BadRequest(remarks);
        //    else return Ok(remarks);
        //}
    }
}
