namespace ScmssApiServer.Models
{
    public class Vendor : Company
    {
        public IList<Supply> Supplies { get; set; }
            = new List<Supply>();

        public IList<PurchaseRequisition> PurchaseRequisitions { get; set; }
            = new List<PurchaseRequisition>();

        public IList<PurchaseOrder> PurchaseOrders { get; set; }
            = new List<PurchaseOrder>();
    }
}
