using System;
using System.Collections.Generic;
using System.Text;

namespace MT.OnlineRestaurant.DataLayer.Context
{
    public partial class TblCart
    {
        public int? Id { get; set; }
        public int? TblCustomerID { get; set; }
        public int? TblRestaurantID { get; set; }
        public int? TblMenuID { get; set; }
        public decimal Price { get; set; }
        public int? Quantity { get; set; }
        public bool status { get; set; }
        public string Itemavailabilitystatus { get; set; }
        public bool offer { get; set; }
        public int UserCreated { get; set; }
        public int UserModified { get; set; }
        public DateTime RecordTimeStamp { get; set; }
        public DateTime RecordTimeStampCreated { get; set; }
    }
}
