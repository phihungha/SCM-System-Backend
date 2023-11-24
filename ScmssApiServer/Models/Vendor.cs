namespace ScmssApiServer.Models
{
    public class Vendor : Company
    {
        public ICollection<Supply> Supplies { get; set; }
            = new List<Supply>();

        public ICollection<PurchaseRequisition> PurchaseRequisitions { get; set; }
            = new List<PurchaseRequisition>();

        public ICollection<PurchaseOrder> PurchaseOrders { get; set; }
            = new List<PurchaseOrder>();
    }
}
