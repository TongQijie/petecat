using Petecat.Data.Entity;

namespace Petecat.Test.Data.Access
{
    public class EggsaverModel
    {
        [PlainDataMapping]
        public string ItemNumber { get; set; }

        [PlainDataMapping]
        public string CountryCode { get; set; }

        [PlainDataMapping]
        public decimal WarehouseAvailableQty { get; set; }

        [PlainDataMapping]
        public string WarehouseNumber { get; set; }

        [PlainDataMapping]
        public decimal TotalAvailableQty { get; set; }

        [PlainDataMapping]
        public string IsEggSaverItem { get; set; }
    }
}
