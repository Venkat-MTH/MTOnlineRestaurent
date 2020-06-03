using Microsoft.EntityFrameworkCore;
using MT.OnlineRestaurant.DataLayer.EntityFrameWorkModel;
using MT.OnlineRestaurant.DataLayer.Repository;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.OnlineRestaurant.UT.DataLayer
{
    [TestFixture]
    class ReviewDataTests
    {
        [Test]
        public void Test_submit_review()
        {
            TblRating reviewdetails = new TblRating()
            {
                TblCustomerId = 1,
                Rating = 5,
                TblRestaurantId = 1,
                Comments = "Good"
            };

            var options = new DbContextOptionsBuilder<ReviewManagementContext>()
            .UseInMemoryDatabase(databaseName: "Reviewmanagement")
            .Options;

            ReviewRepository addreview = new ReviewRepository(new ReviewManagementContext(options));
            int reviewid = addreview.RestaurantRating(reviewdetails);

            Assert.Greater(reviewid, 0);
        }
        [Test]
        public void Update_Test_submit_review()
        {
            TblRating reviewdetails = new TblRating()
            {
                TblCustomerId = 1,
                Rating = 5,
                TblRestaurantId = 1,
                Comments = "Good"
            };

            var options = new DbContextOptionsBuilder<ReviewManagementContext>()
            .UseInMemoryDatabase(databaseName: "Reviewmanagement")
            .Options;

            ReviewRepository addreview = new ReviewRepository(new ReviewManagementContext(options));
            int reviewid = addreview.RestaurantRating(reviewdetails);

            TblRating updatedreviewdetails = new TblRating()
            {
                TblCustomerId = 1,
                Rating = 5,
                TblRestaurantId = 1,
                Comments = "Excellent"
            };

            ReviewRepository Updatereview = new ReviewRepository(new ReviewManagementContext(options));
            int updatedreviewid = Updatereview.RestaurantRating(updatedreviewdetails);

            Assert.AreNotSame(reviewdetails, updatedreviewdetails);
            Assert.AreEqual(reviewid, updatedreviewid);
        }
        [Test]
        public void GetRatingsbyID()
        {
            TblRating reviewdetails = new TblRating()
            {
                TblCustomerId = 1,
                Rating = 5,
                TblRestaurantId = 1,
                Comments = "Good"
            };

            var options = new DbContextOptionsBuilder<ReviewManagementContext>()
            .UseInMemoryDatabase(databaseName: "Reviewmanagement")
            .Options;

            ReviewRepository addreview = new ReviewRepository(new ReviewManagementContext(options));
            int reviewid = addreview.RestaurantRating(reviewdetails);

            ReviewRepository getreview = new ReviewRepository(new ReviewManagementContext(options));
            IQueryable<TblRating> getrating = getreview.GetRestaurantRating(1);
            Assert.AreEqual(1, getrating.Count());
        }
        [Test]
        public void GetRatingsbyCustomerID()
        {
            TblRating reviewdetails = new TblRating()
            {
                TblCustomerId = 1,
                Rating = 5,
                TblRestaurantId = 1,
                Comments = "Good"
            };

            var options = new DbContextOptionsBuilder<ReviewManagementContext>()
            .UseInMemoryDatabase(databaseName: "Reviewmanagement")
            .Options;

            ReviewRepository addreview = new ReviewRepository(new ReviewManagementContext(options));
            int reviewid = addreview.RestaurantRating(reviewdetails);

            ReviewRepository getreview = new ReviewRepository(new ReviewManagementContext(options));
            IQueryable<TblRating> getrating = getreview.GetRestaurantRatingByCustomer(1);
            Assert.AreEqual(1, getrating.Count());
        }
        
    }
}
