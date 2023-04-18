namespace RFIDP2P3_API.Models
{
    public class Report
    {

        public string? ReportType { get; set; }
        public string? EmptyPalet { get; set; }
        public string? BufferStock  { get; set; }
        public string? PreDelivery  { get; set; }
        public string? DeliveryCasting  { get; set; }
        public string? Receiving  { get; set; }
        public string? Posting  { get; set; }
        public string? DeliveryEngine { get; set; }
        public string? MaterialNo  { get; set; }
        public string? MaterialName    { get; set; } 
        public string? MaterialDesc  { get; set; }
        public string? PairingDate  { get; set; }    
        public string? KanbanNo  { get; set; }
        public string? ReceivingCDate { get; set; }
        public string? IDPallet  { get; set; }
        public string? DNNo  { get; set; }   
        public string? EmptyPalletDate  { get; set; }
        public string? BufferStockDate  { get; set; }
        public string? PreDeliveryDate  { get; set; }
        public string? DeliveryCDate  { get; set; }  
        public string? ReceivingEDate  { get; set; }
        public string? DeliveryEDate  { get; set; }  
        public string? PostingDate { get; set; }

    }
}
