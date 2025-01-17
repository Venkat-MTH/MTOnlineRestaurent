﻿using MT.OnlineRestaurant.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.OnlineRestaurant.BusinessLayer.interfaces
{
    public interface IPlaceOrderActions
    {
        int PlaceOrder(OrderEntity orderEntity);
        int CancelOrder(int orderId);

        /// <summary>
        /// gets the customer placed order details
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        IQueryable<CustomerOrderReport> GetReports(int customerId);
         OrderEntity mappingorderandcartitems(int userid);
        Task<bool> IsValidRestaurantAsync(OrderEntity orderEntity, int UserId, string UserToken);
        Task<bool> IsValidOfferAsync(OrderEntity orderEntity, int UserId, string UserToken);
        Task<bool> IsOrderItemInStock(OrderEntity orderEntity, int UserId, string UserToken);

    }
}
