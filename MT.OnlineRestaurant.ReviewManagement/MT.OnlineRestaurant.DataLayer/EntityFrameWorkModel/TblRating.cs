using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MT.OnlineRestaurant.DataLayer.EntityFrameWorkModel
{
    [ExcludeFromCodeCoverage]
    public partial class TblRating
    {
        public int Rating { get; set; }
        public string Comments { get; set; }
        public int TblRestaurantId { get; set; }
        public int Id { get; set; }
        public int TblCustomerId { get; set; }
        public int UserCreated { get; set; }
        public int UserModified { get; set; }
        public DateTime RecordTimeStamp { get; set; }
        public DateTime RecordTimeStampCreated { get; set; }

        
    }
}
