using System;
using System.Collections.Generic;
using System.Text;
using MT.OnlineRestaurant.DataLayer.EntityFrameWorkModel;
using System.Linq;
using MT.OnlineRestaurant.DataLayer.DataEntity;
using Microsoft.Extensions.Options;
using MT.OnlineRestaurant.DataLayer.Repository;

namespace MT.OnlineRestaurant.BusinessLayer.Repository
{
    public class MenuPriceBusiness : IMenuPriceBusiness
    {
        private readonly IMenuPriceRepository menupricerepo;
        public MenuPriceBusiness(IMenuPriceRepository _menupricerepo)
        {
            menupricerepo = _menupricerepo;
        }


        #region Interface Methods

        /// <summary>
        /// Menu Price Change
        /// </summary>
        /// <param name="menuID"></param>
        /// <param name="price"></param>

        public TblMenu MenuPriceUpdate(int menuID,int price)
        {
            try
            {
                TblMenu tblobj = new TblMenu();
                tblobj = menupricerepo.MenuPriceUpdate(menuID, price);
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
