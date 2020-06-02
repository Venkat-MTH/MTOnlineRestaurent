
using MT.OnlineRestaurant.DataLayer.EntityFrameWorkModel;
using System.Collections.Generic;
using System.Linq;

namespace MT.OnlineRestaurant.DataLayer.Repository
{
    public interface IReviewRepository
    {
        /// <summary>
        /// Recording the customer rating the restaurants
        /// </summary>
        /// <param name="tblRating"></param>
        int RestaurantRating(TblRating tblRating);
        IQueryable<TblRating> GetRestaurantRating(int restaurantID);
        IQueryable<TblRating> GetRestaurantRatingByCustomer(int customerID);
    }
}
