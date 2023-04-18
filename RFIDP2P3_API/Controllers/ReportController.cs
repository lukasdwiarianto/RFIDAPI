using Microsoft.AspNetCore.Mvc;
using RFIDP2P3_API.Models;
using System.Data;
using System.Data.SqlClient;

namespace RFIDP2P3_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class reportController : ControllerBase
    {
        private readonly string _configuration;
        private string remarks = "";

        public reportController(IConfiguration configuration)
        {
            _configuration = configuration.GetConnectionString("DefaultConnection");
        }
        // wajib 
        [HttpPost]
        public ActionResult<IEnumerable<Report>> INQ(Report ReportParam)
        {
            List<Report> ReportResult = new();
           
            var ReportType = ReportParam.ReportType;
            if (string.IsNullOrEmpty(ReportType))
            {
                ReportType = "#grid1";
            }

            using (SqlConnection conn = new(_configuration))
            {
                conn.Open();
                SqlCommand cmd = new("exec sp_Inq_Report_Summary @grid", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new("@grid", ReportType));

                SqlDataReader sdr = cmd.ExecuteReader();
                var vessel = new Report { };
                while (sdr.Read())
                {
                    switch (ReportType)
                    {
                        case "#grid1":
                            var totalPalet = sdr["TotalPalet"].ToString();
                            var locid = sdr["Locid"].ToString();
                            switch (locid)
                            {
                                case "1": vessel.EmptyPalet = totalPalet;break;
                                case "2": vessel.BufferStock = totalPalet;break;
                                case "3": vessel.PreDelivery = totalPalet;break;
                                case "4": vessel.DeliveryCasting = totalPalet;break;
                                case "5": vessel.Receiving = totalPalet;break;
                                case "6": vessel.Posting = totalPalet;break;
                                default: vessel.DeliveryEngine = totalPalet;break;
                            }
                            break;
                        case "#grid2":
                            ReportResult.Add(new Report
                            {
                                IDPallet = sdr["IDPallet"].ToString(),
                                DNNo = sdr["DNNo"].ToString(),
                                EmptyPalletDate = sdr["EmptyPalletDate"].ToString(),
                                BufferStockDate = sdr["BufferStockDate"].ToString(),
                                PreDeliveryDate = sdr["PreDeliveryDate"].ToString(),
                                DeliveryCDate = sdr["DeliveryCDate"].ToString(),
                                ReceivingEDate = sdr["ReceivingEDate"].ToString(),
                                DeliveryEDate = sdr["DeliveryEDate"].ToString(),
                                PostingDate = sdr["PostingDate"].ToString()
                            });
                            break;
                        case "#grid3":
                            ReportResult.Add(new Report
                            {
                                IDPallet = sdr["IDPallet"].ToString(),
                                ReceivingCDate = sdr["ReceivingCDate"].ToString()
                            });
                            break;
                        case "#grid4":
                            ReportResult.Add(new Report
                            {
                                IDPallet = sdr["IDPallet"].ToString(),
                                MaterialNo = sdr["MaterialNo"].ToString(),
                                MaterialName = sdr["MaterialName"].ToString(),
                                MaterialDesc = sdr["MaterialDecsription"].ToString(),
                                PairingDate = sdr["PairingDate"].ToString()
                            });
                            break;
                        case "#grid5":
                            ReportResult.Add(new Report
                            {
                                MaterialNo = sdr["MaterialNo"].ToString(),
                                MaterialName = sdr["MaterialName"].ToString(),
                                MaterialDesc = sdr["MaterialDecsription"].ToString(),
                                PairingDate = sdr["PairingDate"].ToString(),
                                KanbanNo = sdr["KanbanNo"].ToString(),
                                IDPallet = sdr["IDPallet"].ToString(),
                                DNNo = sdr["DNNo"].ToString()

                            });
                            break;
                        case "#grid6":
                            ReportResult.Add(new Report
                            {
                                KanbanNo = sdr["KanbanNo"].ToString(),
                                IDPallet = sdr["IDPallet"].ToString(),
                                DNNo = sdr["DNNo"].ToString(),
                                DeliveryCDate = sdr["DeliveryCDate"].ToString()

                            });
                            break;
                        case "#grid7":
                            ReportResult.Add(new Report
                            {
                                KanbanNo = sdr["KanbanNo"].ToString(),
                                IDPallet = sdr["IDPallet"].ToString(),
                                DNNo = sdr["DNNo"].ToString(),
                                ReceivingEDate = sdr["ReceivingEDate"].ToString()
                            });
                            break;
                        case "#grid8":
                            ReportResult.Add(new Report
                            {
                                IDPallet = sdr["IDPallet"].ToString(),
                                MaterialNo = sdr["MaterialNo"].ToString(),
                                MaterialName = sdr["MaterialName"].ToString(),
                                MaterialDesc = sdr["MaterialDecsription"].ToString(),
                                PostingDate = sdr["PostingDate"].ToString()
                            });
                            break;
                        default:
                            ReportResult.Add(new Report
                            {
                                IDPallet = sdr["IDPallet"].ToString(),
                                DeliveryEDate = sdr["DeliveryEDate"].ToString()
                            });
                            break;
                    }
                }
                if (ReportType == "#grid1"){
                    ReportResult.Add(vessel);
                }
                conn.Close();
            }
            return ReportResult;
        }


    }
}
