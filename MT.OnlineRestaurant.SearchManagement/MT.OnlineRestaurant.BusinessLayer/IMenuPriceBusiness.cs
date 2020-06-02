using MT.OnlineRestaurant.DataLayer.DataEntity;
using MT.OnlineRestaurant.DataLayer.EntityFrameWorkModel;
using System.Collections.Generic;
using System.Linq;

namespace MT.OnlineRestaurant.BusinessLayer.Repository
{
    public interface IMenuPriceBusiness
    {
        TblMenu MenuPriceUpdate(int MenuID,int price);
    }
}
