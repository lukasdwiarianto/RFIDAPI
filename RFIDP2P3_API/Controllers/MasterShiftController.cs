using Microsoft.AspNetCore.Mvc;
using RFIDP2P3_API.Models;
using System.Data.SqlClient;
using System.Data;

namespace RFIDP2P3_API.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class MasterShiftController : Controller
	{
		private readonly string _configuration;
		private string remarks = "";

		public MasterShiftController(IConfiguration configuration)
		{
			_configuration = configuration.GetConnectionString("DefaultConnection");
		}

		[HttpPost]
		public ActionResult<IEnumerable<MasterShift>> INQ()
		{
			List<MasterShift> ContainerObj = new();

            using (SqlConnection conn = new SqlConnection(_configuration))
            using (SqlCommand cmd = new SqlCommand("sp_M_Shift_Sel", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();

				while (sdr.Read())
				{
					ContainerObj.Add(new MasterShift
					{
						PlantID = sdr["PlantID"].ToString(),
						ShiftID = sdr["ShiftID"].ToString(),
						DayShiftEnd = sdr["DayShiftEnd"].ToString(),
						DayShiftStart = sdr["DayShiftStart"].ToString(),
						NightShiftEnd = sdr["NightShiftEnd"].ToString(),
						NightShiftStart = sdr["NightShiftStart"].ToString(),
						LastUpdate = sdr["LastUpdate"].ToString(),
						UserUpdate = sdr["UserUpdate"].ToString(),
						Remarks = sdr["Remarks"].ToString()

					});
				}
				conn.Close();
			}
			return ContainerObj;
		}

		[HttpPost]
		public ActionResult<IEnumerable<MasterShift>> INS(MasterShift paramObj)
		{
            using (SqlConnection conn = new SqlConnection(_configuration))
            using (SqlCommand cmd = new SqlCommand("sp_M_Shift_Ins", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

				cmd.Parameters.Add(new("@IUType", paramObj.IUType));
				cmd.Parameters.Add(new("@PlantID", paramObj.PlantID));
				cmd.Parameters.Add(new("@ShiftID", paramObj.ShiftID));
				cmd.Parameters.Add(new("@DShiftStart", paramObj.DayShiftStart));
				cmd.Parameters.Add(new("@DShiftEnd", paramObj.DayShiftEnd));
				cmd.Parameters.Add(new("@NShiftStart", paramObj.NightShiftStart));
				cmd.Parameters.Add(new("@NShiftEnd", paramObj.NightShiftEnd));
				cmd.Parameters.Add(new("@UserLogin", paramObj.UserLogin));

                conn.Open();
                cmd.ExecuteNonQuery();
                remarks = Convert.ToString(cmd.Parameters["@Remarks"].Value);
                conn.Close();
            }
			if (remarks != "") return BadRequest(remarks);
			else return Ok("success");
		}


		[HttpPost]
		public ActionResult<IEnumerable<MasterShift>> DEL(MasterShift paramObj)
		{
            using (SqlConnection conn = new SqlConnection(_configuration))
            using (SqlCommand cmd = new SqlCommand("sp_M_Shift_Del", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                cmd.Parameters.Add(new("@ShiftID", paramObj.ShiftID));

                conn.Open();
                cmd.ExecuteNonQuery();
                remarks = Convert.ToString(cmd.Parameters["@Remarks"].Value);
                conn.Close();
            }
			if (remarks != "") return BadRequest(remarks);
			else return Ok("success");
		}

	}
}
