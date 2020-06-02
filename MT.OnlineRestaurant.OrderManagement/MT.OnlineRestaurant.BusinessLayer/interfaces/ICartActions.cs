#region References
using MT.OnlineRestaurant.BusinessEntities;
using MT.OnlineRestaurant.DataLayer.Context;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

#region namespace
namespace MT.OnlineRestaurant.BusinessLayer.interfaces
{
    #region Interface Definition
    /// <summary>
    /// Defines data actions for Cart
    /// </summary>
    public interface ICartActions
    {
        int AddCart(CartItemsEntity tblcart);

        /// <summary>
        /// Update Cart details by CartDetailsId
        /// </summary>
        /// <param name="CartID"></param>
        /// <returns></returns>
       void UpdateCartitemstatus(CartItemsEntity tblcart);

        /// <summary>
        /// Get Cart details by CustomerID
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        List<CartItemsEntity> GetCartDetails(int CustomerID);
        Task UpdateCartMenuItemPrice(CartItemsEntity tblcart);
        Task UpdateItemoutofstock(string msg);
        void Removecart(int customerID,int? menuID);
    }
    #endregion
}
#endregion