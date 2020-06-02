using MT.OnlineRestaurant.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.OnlineRestaurant.BusinessLayer
{
    public interface IReviewBusiness
    {
        /// <summary>
        /// Recording the customer rating the restaurants
        /// </summary>
        /// <param name=""></param>
        int RestaurantRating(RestaurantRating restaurantRating);
        IQueryable<RestaurantRating> GetRestaurantRating(int restaurantID);
        IQueryable<RestaurantRating> GetRestaurantRatingByCustomer(int customerID);
    }
}
