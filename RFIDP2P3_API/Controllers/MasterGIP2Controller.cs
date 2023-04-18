using Microsoft.AspNetCore.Mvc;
using RFIDP2P3_API.Models;
using System.Data.SqlClient;
using System.Data;

namespace RFIDP2P3_API.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class MasterGIP2Controller : Controller
	{
		private readonly string _configuration;
		private string remarks = "";

		public MasterGIP2Controller(IConfiguration configuration)
		{
			_configuration = configuration.GetConnectionString("DefaultConnection");
		}

		[HttpPost]
		public ActionResult<IEnumerable<MasterGIP2>> INQ()
		{
			List<MasterGIP2> ContainerObj = new();

            using (SqlConnection conn = new SqlConnection(_configuration))
            using (SqlCommand cmd = new SqlCommand("sp_M_GIP2_Sel", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
				{
					ContainerObj.Add(new MasterGIP2
					{
						GIP2Id  = sdr["GIP2Id"].ToString(),
						PlantId  = sdr["PlantId"].ToString(),
						PlantName  = sdr["PlantName"].ToString(),
						ShopId  = sdr["ShopId"].ToString(),
						ShopName  = sdr["ShopName"].ToString(),
						DeptId  = sdr["DeptId"].ToString(),
						DeptName  = sdr["DeptName"].ToString(),
						CostCenter  = sdr["CostCenter"].ToString(),
						UserUpdate  = sdr["UserUpdate"].ToString(),
						DateUpdate = sdr["DateUpdate"].ToString()


					});
				}
				conn.Close();
			}
			return ContainerObj;
		}

		[HttpPost]
		public ActionResult<IEnumerable<MasterGIP2>> INS(MasterGIP2 paramObj)
		{
            using (SqlConnection conn = new SqlConnection(_configuration))
            using (SqlCommand cmd = new SqlCommand("sp_M_GIP2_Ins", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                cmd.Parameters.Add(new("@IUType", paramObj.IUType));
				cmd.Parameters.Add(new("@GIP2Id", paramObj.GIP2Id));
				cmd.Parameters.Add(new("@DeptId", paramObj.DeptId));
				cmd.Parameters.Add(new("@CostCenter", paramObj.CostCenter));
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
		public ActionResult<IEnumerable<MasterGIP2>> UPL(MasterGIP2 paramObj)
		{
			//using (SqlConnection conn = new(_configuration))
			//{
			//	conn.Open();
			//	SqlCommand cmd = new("exec sp_M_GIP2_Ins @PartID, @PlantID, @BuildingID, @BuildingName, @PartNumber, @PartName, @PartDesc, @CycleTimeMin, @CycleTimeSec, @Efficiency, @LineID, @LineName, @QtyperPallet, @QtyProdPerDay, @PartStockMin, @PartStockMax, @PartStockDay, @CycleMax, @LastUpdate, @UserUpdate, @Remarks", conn);
			//	cmd.CommandType = CommandType.StoredProcedure;
			//	cmd.Parameters.Add(new("PartID", paramObj.PartID));
			//	cmd.Parameters.Add(new("PlantID", paramObj.PlantID));
			//	cmd.Parameters.Add(new("BuildingID", paramObj.BuildingID));
			//	cmd.Parameters.Add(new("BuildingName", paramObj.BuildingName));
			//	cmd.Parameters.Add(new("PartNumber", paramObj.PartNumber));
			//	cmd.Parameters.Add(new("PartName", paramObj.PartName));
			//	cmd.Parameters.Add(new("PartDesc", paramObj.PartDesc));
			//	cmd.Parameters.Add(new("CycleTimeMin", paramObj.CycleTimeMin));
			//	cmd.Parameters.Add(new("CycleTimeSec", paramObj.CycleTimeSec));
			//	cmd.Parameters.Add(new("Efficiency", paramObj.Efficiency));
			//	cmd.Parameters.Add(new("LineID", paramObj.LineID));
			//	cmd.Parameters.Add(new("LineName", paramObj.LineName));
			//	cmd.Parameters.Add(new("QtyperPallet", paramObj.QtyperPallet));
			//	cmd.Parameters.Add(new("QtyProdPerDay", paramObj.QtyProdPerDay));
			//	cmd.Parameters.Add(new("PartStockMin", paramObj.PartStockMin));
			//	cmd.Parameters.Add(new("PartStockMax", paramObj.PartStockMax));
			//	cmd.Parameters.Add(new("PartStockDay", paramObj.PartStockDay));
			//	cmd.Parameters.Add(new("CycleMax", paramObj.CycleMax));
			//	cmd.Parameters.Add(new("LastUpdate", paramObj.LastUpdate));
			//	cmd.Parameters.Add(new("UserUpdate", paramObj.UserUpdate));
			//	cmd.Parameters.Add(new("Remarks", paramObj.Remarks));

			//	remarks = cmd.ExecuteScalar().ToString();
			//	conn.Close();
			//}
			if (remarks != "") return BadRequest(remarks);
			else return Ok("success");
		}


		[HttpPost]
		public ActionResult<IEnumerable<MasterGIP2>> DEL(MasterGIP2 paramObj)
		{
            using (SqlConnection conn = new SqlConnection(_configuration))
            using (SqlCommand cmd = new SqlCommand("sp_M_GIP2_Del", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                cmd.Parameters.Add(new("@GIP2ID", paramObj.GIP2Id));

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
