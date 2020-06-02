#region References
using MT.OnlineRestaurant.DataLayer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

#region namespace
namespace MT.OnlineRestaurant.DataLayer.interfaces
{
    #region Interface Definition
    /// <summary>
    /// Defines data actions for Cart
    /// </summary>
    public interface ICartRepository
    {
        int AddCart(TblCart tblcart);

        /// <summary>
        /// Update Cart details by CartDetailsId
        /// </summary>
        /// <param name="CartID"></param>
        /// <returns></returns>
        int UpdateCart(int CartID);
        
        /// <summary>
        /// Get Cart details by CustomerID
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        IQueryable<TblCart> GetCartDetails(int CustomerID);
        Task UpdateCartMenuItemPrice(TblCart tblcart);
        Task UpdateItemoutofstock(int? menuId);
        int RemoveCart(int customerID, int? menuID);
        void UpdateCartitemstatus(TblCart tblcart);
    }
    #endregion
}
#endregion