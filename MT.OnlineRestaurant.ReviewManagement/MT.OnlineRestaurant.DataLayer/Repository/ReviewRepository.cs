using System;
using System.Collections.Generic;
using System.Text;
using MT.OnlineRestaurant.DataLayer.EntityFrameWorkModel;
using System.Linq;

using Microsoft.Extensions.Options;

namespace MT.OnlineRestaurant.DataLayer.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ReviewManagementContext db;
        public ReviewRepository(ReviewManagementContext connection)
        {
            db = connection;
        }

        #region Interface Methods
     
        /// <summary>
        /// Recording the customer rating the restaurants
        /// </summary>
        /// <param name="tblRating"></param>
        public int RestaurantRating(TblRating tblRating)
        {
            TblRating tr = db.TblRating.Where(r=>r.TblRestaurantId == tblRating.TblRestaurantId && r.TblCustomerId == tblRating.TblCustomerId).FirstOrDefault();
            if (tr != null)
            {
                tr.Comments = tblRating.Comments;
                tr.Rating = tblRating.Rating;
                tr.RecordTimeStamp = DateTime.Now;
                tr.RecordTimeStampCreated = DateTime.Now;
                tr.TblCustomerId = tblRating.TblCustomerId;
                tr.TblRestaurantId = tblRating.TblRestaurantId;
                db.SaveChanges();
                return tr.Id;
            }
            else { 
                db.Set<TblRating>().Add(tblRating);
                db.SaveChanges();
                return tblRating.Id;
            }
        }
        public IQueryable<TblRating> GetRestaurantRating(int restaurantID)
        {
            // List<TblRating> restaurant_Rating = new List<TblRating>();
            try
            {
                
                    return (from rating in db.TblRating
                            where rating.TblRestaurantId == restaurantID
                            select new TblRating
                            {
                                Rating = rating.Rating,
                                Comments = rating.Comments,
                                TblCustomerId = rating.TblCustomerId,
                                TblRestaurantId= rating.TblRestaurantId
                            }).AsQueryable();
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<TblRating> GetRestaurantRatingByCustomer(int customerID)
        {
            try
            {
                
                    return (from rating in db.TblRating
                            where rating.TblCustomerId == customerID
                            select new TblRating
                            {
                                Rating = rating.Rating,
                                Comments = rating.Comments,
                                TblCustomerId = rating.TblCustomerId,
                                TblRestaurantId = rating.TblRestaurantId
                            }).AsQueryable();
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
