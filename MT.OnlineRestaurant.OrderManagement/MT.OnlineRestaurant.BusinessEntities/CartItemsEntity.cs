using System;
using System.Collections.Generic;
using System.Text;

namespace MT.OnlineRestaurant.BusinessEntities
{
    public class CartItemsEntity
    {
       // public int? CartId { get; set; }
        public int? TblCustomerID { get; set; }
        public int? TblRestaurantID { get; set; }
        public int? TblMenuID { get; set; }
        public decimal Price { get; set; }
        public int? Quantity { get; set; }
        // Item availability
        public bool Status { get; set; }
        public string Itemavailabilitystatus { get; set; }
        public bool offer { get; set; }
        public DateTime RecordTimeStamp { get; set; }
        public DateTime RecordTimeStampCreated { get; set; }
    }
}
