using System;
using System.Collections.Generic;
using System.Text;
using MT.OnlineRestaurant.DataLayer.EntityFrameWorkModel;
using System.Linq;
using MT.OnlineRestaurant.DataLayer.DataEntity;
using Microsoft.Extensions.Options;

namespace MT.OnlineRestaurant.DataLayer.Repository
{
    public class MenuPriceRepository : IMenuPriceRepository
    {
        private readonly RestaurantManagementContext db;
        public MenuPriceRepository(RestaurantManagementContext connection)
        {
            db = connection;
        }
       

        #region Interface Methods
       
        /// <summary>
        /// Menu Price Change
        /// </summary>
        /// <param name="menuID"></param>
       
        public TblMenu MenuPriceUpdate(int menuID,int price)
        {
            try
            {
                TblMenu tblobj = new TblMenu();
                if (db != null)
                {
                    tblobj = db.TblMenu.Where(m => m.Id == menuID).FirstOrDefault();
                    if (tblobj !=null)
                    {
                        tblobj.price = price;
                        db.Set<TblMenu>().Update(tblobj);
                        db.SaveChanges();
                    }
                }
                return tblobj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        
    }

}
