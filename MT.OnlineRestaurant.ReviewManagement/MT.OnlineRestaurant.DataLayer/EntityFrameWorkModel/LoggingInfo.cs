using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MT.OnlineRestaurant.DataLayer.EntityFrameWorkModel
{
    [ExcludeFromCodeCoverage]
    public partial class LoggingInfo
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public DateTime? RecordTimeStamp { get; set; }
    }
}
