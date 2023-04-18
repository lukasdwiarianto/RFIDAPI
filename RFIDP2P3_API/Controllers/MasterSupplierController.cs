using Microsoft.AspNetCore.Mvc;
using RFIDP2P3_API.Models;
using System.Data;
using System.Data.SqlClient;

namespace RFIDP2P3_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MasterSupplierController : ControllerBase
    {
        private readonly string _configuration;
        private string remarks = "";

        public MasterSupplierController(IConfiguration configuration)
        {
            _configuration = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpPost]
        public ActionResult<IEnumerable<MasterSupplier>> INQ()
        {
            List<MasterSupplier> Suppliers = new();

            using (SqlConnection conn = new(_configuration))
            {
                conn.Open();
                SqlCommand cmd = new("SELECT a.*, b.Area_Name FROM T_Supplier_Master a LEFT JOIN T_Master_Area b ON a.Area_Code = b.Area_Code ORDER BY Supplier_Code", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    Suppliers.Add(new MasterSupplier
                    {
                        Supplier_Code = sdr["Supplier_Code"].ToString(),
                        Supplier_Name = sdr["Supplier_Name"].ToString(),
                        Supplier_Email = sdr["Supplier_Email"].ToString(),
                        Area_Code = sdr["Area_Code"].ToString(),
                        Area_Name = sdr["Area_Name"].ToString()
                    });
                }
                conn.Close();
            }
            return Suppliers;
        }

        [HttpPost]
        public ActionResult<IEnumerable<MasterSupplier>> DEL(MasterSupplier supplier)
        {
            using (SqlConnection conn = new(_configuration))
            {
                conn.Open();
                SqlCommand cmd = new("exec sp_Delete_T_Supplier_Master @Supplier_Code", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new("@Supplier_Code", supplier.Supplier_Code));
                remarks = cmd.ExecuteScalar().ToString();
                conn.Close();
            }
            if (remarks != "success") return BadRequest(remarks);
            else return Ok(remarks);
        }

        [HttpPost]
        public ActionResult<IEnumerable<MasterSupplier>> INS(MasterSupplier supplier)
        {
            using (SqlConnection conn = new(_configuration))
            {
                conn.Open();
                SqlCommand cmd = new("EXEC sp_Submit_T_Supplier_Master @Supplier_Code, @Supplier_Name, @Supplier_Email, @Area_Code, @UserLogin", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new("@Supplier_Code", supplier.Supplier_Code));
                cmd.Parameters.Add(new("@Supplier_Name", supplier.Supplier_Name));
                cmd.Parameters.Add(new("@Supplier_Email", supplier.Supplier_Email));
                cmd.Parameters.Add(new("@Area_Code", supplier.Area_Code));
                cmd.Parameters.Add(new("@UserLogin", supplier.UserLogin));
                remarks = cmd.ExecuteScalar().ToString();
                conn.Close();
            }
            if (remarks != "success") return BadRequest(remarks);
            else return Ok(remarks);
        }
    }
}
