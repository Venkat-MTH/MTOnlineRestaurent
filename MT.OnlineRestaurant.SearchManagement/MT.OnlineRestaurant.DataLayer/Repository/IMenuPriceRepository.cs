using MT.OnlineRestaurant.DataLayer.DataEntity;
using MT.OnlineRestaurant.DataLayer.EntityFrameWorkModel;
using System.Collections.Generic;
using System.Linq;

namespace MT.OnlineRestaurant.DataLayer.Repository
{
    public interface IMenuPriceRepository
    {
        TblMenu MenuPriceUpdate(int MenuID,int price);
    }
}
