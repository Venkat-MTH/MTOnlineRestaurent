using MT.OnlineRestaurant.BusinessEntities;
using System;
using System.Collections.Generic;
using MT.OnlineRestaurant.DataLayer.Repository;
using MT.OnlineRestaurant.DataLayer.EntityFrameWorkModel;
using System.Text;
using System.Linq;

namespace MT.OnlineRestaurant.BusinessLayer
{
    public class ReviewBusiness : IReviewBusiness
    {
        IReviewRepository review_Repository;
        private readonly string connectionstring;
        public ReviewBusiness(IReviewRepository _reviewRepository)
        {
            review_Repository = _reviewRepository;
        }


        /// <summary>
        /// Recording the customer rating the restaurants
        /// </summary>
        /// <param name="restaurantRating"></param>
        public int RestaurantRating(RestaurantRating restaurantRating)
        {
            if (restaurantRating != null)
            {
                TblRating rating = new TblRating()
                {
                    Rating = restaurantRating.rating,
                    TblRestaurantId = restaurantRating.RestaurantId,
                    Comments = restaurantRating.user_Comments,
                    TblCustomerId = restaurantRating.customerId
                };

              return review_Repository.RestaurantRating(rating);
            }
            return 0;
        }

        public IQueryable<RestaurantRating> GetRestaurantRating(int restaurantID)
        {
            
                List<RestaurantRating> restaurantRatings = new List<RestaurantRating>();
                IQueryable<TblRating> rating;
                rating = review_Repository.GetRestaurantRating(restaurantID);
                if  (rating.Count() == 0)
                {
                    return null;
                }
                foreach (var item in rating)
                {
                    RestaurantRating ratings = new RestaurantRating
                    {
                        RestaurantId = item.TblRestaurantId,
                        rating = item.Rating,
                        user_Comments = item.Comments,
                        customerId = item.TblCustomerId
                    };
                    restaurantRatings.Add(ratings);
                }
                return restaurantRatings.AsQueryable();
          
        }
        public IQueryable<RestaurantRating> GetRestaurantRatingByCustomer(int customerID)
        {
              List<RestaurantRating> restaurantRatings = new List<RestaurantRating>();
                IQueryable<TblRating> rating;
                rating = review_Repository.GetRestaurantRatingByCustomer(customerID);
                foreach (var item in rating)
                {
                    RestaurantRating ratings = new RestaurantRating
                    {
                        RestaurantId = item.TblRestaurantId,
                        rating = item.Rating,
                        user_Comments = item.Comments,
                        customerId = item.TblCustomerId
                    };
                    restaurantRatings.Add(ratings);
                }
                return restaurantRatings.AsQueryable();
            
        }
    }
}
