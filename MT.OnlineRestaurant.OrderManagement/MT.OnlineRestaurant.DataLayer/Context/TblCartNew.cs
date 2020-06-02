using System;
using System.Collections.Generic;
using System.Text;

namespace MT.OnlineRestaurant.DataLayer.Context
{
    public partial class TblCartNew
    {
        public TblCartNew()
        {
            TblCartDetails = new HashSet<TblCartDetails>();
        }
        public int? Id { get; set; }
        public int? TblCustomerID { get; set; }
        public int? TblRestaurantID { get; set; }
        ICollection<TblCartDetails> CartDetails { get; set; }
        public int UserCreated { get; set; }
        public int UserModified { get; set; }
        public DateTime RecordTimeStamp { get; set; }
        public DateTime RecordTimeStampCreated { get; set; }
        public virtual ICollection<TblCartDetails> TblCartDetails { get; set; }
    }
}
