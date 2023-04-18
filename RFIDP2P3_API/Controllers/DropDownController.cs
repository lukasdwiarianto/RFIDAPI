using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using RFIDP2P3_API.Models;

namespace RFIDP2P3_API.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class DropDownController : Controller
	{
		private readonly string _configuration;

		public DropDownController(IConfiguration configuration)
		{
			_configuration = configuration.GetConnectionString("DefaultConnection");
		}

		[HttpPost]
		public ActionResult<IEnumerable<DropDown>> INQ(string Ftype, string PlantId, string BuildingId, string ShopId, string DeptID)
		{
			List<DropDown> ContainerObj = new();

            using (SqlConnection conn = new SqlConnection(_configuration))
            using (SqlCommand cmd = new SqlCommand("sp_Dropdown_Sel", conn))
            {
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add(new("@Ftype", Ftype));
				cmd.Parameters.Add(new("@PlantId", PlantId));
                cmd.Parameters.Add(new("@BuildingId", BuildingId));
                cmd.Parameters.Add(new("@ShopId", ShopId));
                cmd.Parameters.Add(new("@DeptId", DeptID));

                conn.Open();
				SqlDataReader sdr = cmd.ExecuteReader();
				while (sdr.Read())
				{
					switch (Ftype)
					{
						case "Plant":
							ContainerObj.Add(new DropDown
							{
								PlantID = sdr["PlantID"].ToString(),
								PlantName = sdr["PlantName"].ToString(),

							});break;
						case "Gedung":
							ContainerObj.Add(new DropDown
							{
								BuildingId = sdr["BuildingId"].ToString(),
								BuildingName = sdr["BuildingName"].ToString()

							});break;
						case "Shop":
							ContainerObj.Add(new DropDown
							{
								ShopId = sdr["ShopId"].ToString(),
								ShopName = sdr["ShopName"].ToString()

							}); break;
						case "Dept":
							ContainerObj.Add(new DropDown
							{
								DeptID = sdr["DeptID"].ToString(),
								DeptName = sdr["DeptName"].ToString()

							}); break;
						case "PalletType":
							ContainerObj.Add(new DropDown
							{
								PalletTypeID = sdr["PalletTypeID"].ToString(),
								PalletTypeName = sdr["PalletTypeName"].ToString()

							}); break;
					}
					
				}
				conn.Close();
			}
			return ContainerObj;
		}
	}
}
