using Petecat.Data.Attributes;
using Petecat.Data.Entity;

namespace Petecat.Test.Data.Access
{
    public class SO
    {
        [PlainDataMapping]
        public int SONumber { get; set; }

        [PlainDataMapping]
        public int IndexName { get; set; }

        [PlainDataMapping]
        public int SOType { get; set; }

        [PlainDataMapping]
        public string ItemNumber { get; set; }

        [PlainDataMapping]
        public int Quantity { get; set; }

        [PlainDataMapping]
        public int ShipViaCode { get; set; }

        [PlainDataMapping]
        public string WarehouseNumber { get; set; }

        [PlainDataMapping]
        public string SellerID { get; set; }

        [PlainDataMapping]
        public string SellerName { get; set; }

        [PlainDataMapping]
        public string Description { get; set; }

        [PlainDataMapping]
        public decimal ExtendPrice { get; set; }
    }
}
