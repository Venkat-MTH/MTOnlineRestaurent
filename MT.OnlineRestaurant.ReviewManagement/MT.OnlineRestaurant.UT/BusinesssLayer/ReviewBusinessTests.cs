
using Moq;
using MT.OnlineRestaurant.BusinessEntities;
using MT.OnlineRestaurant.BusinessLayer;
using MT.OnlineRestaurant.DataLayer.EntityFrameWorkModel;
using MT.OnlineRestaurant.DataLayer.Repository;
using MT.OnlineRestaurant.ReviewManagement.Controllers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace MT.OnlineRestaurant.UT.BusinesssLayer
{
    [TestFixture]
    class ReviewBusinessTests
    {
        [Test]
        public void Test_Rating_submit()
        {
            RestaurantRating reviewdetails = new RestaurantRating()
            {
                customerId = 1,
                rating = 5,
                RestaurantId = 1,
                user_Comments = "Good"
            };

            var mockOrder = new Mock<IReviewRepository>();
            mockOrder.Setup(x => x.RestaurantRating(It.IsAny<TblRating>())).Returns(1);
            var orderFoodActionObject = new ReviewBusiness(mockOrder.Object);
            var data= orderFoodActionObject.RestaurantRating(reviewdetails);
            Assert.AreEqual(1, data);
        }

        [Test]
        public void Nodata_Test_Rating_submit()
        {
            RestaurantRating reviewdetails = new RestaurantRating();
            reviewdetails = null;

            var mockOrder = new Mock<IReviewRepository>();
            mockOrder.Setup(x => x.RestaurantRating(It.IsAny<TblRating>())).Returns(1);
            var orderFoodActionObject = new ReviewBusiness(mockOrder.Object);
            var data = orderFoodActionObject.RestaurantRating(reviewdetails);
            Assert.AreEqual(0, data);
        }

        [Test]
        public void Invalid_Rating_submit()
        {
            RestaurantRating reviewdetails = new RestaurantRating()
            {
                customerId = 1,
                rating = 5,
                RestaurantId = 1,
                user_Comments = "Good"
            };

            var mockOrder = new Mock<IReviewRepository>();
            mockOrder.Setup(x => x.RestaurantRating(It.IsAny<TblRating>())).Returns(0);
            var orderFoodActionObject = new ReviewBusiness(mockOrder.Object);
            var data = orderFoodActionObject.RestaurantRating(reviewdetails);
            Assert.AreEqual(0, data);
        }

        [Test]
        public void Getratingsbyid()
        {
            List<TblRating> restaurantRatings = new List<TblRating>();
            restaurantRatings.Add(new TblRating()
            {
                TblRestaurantId = 1,
                TblCustomerId = 1,
                Rating = 5,
                Comments = "Not Bad",
            });

            var mockOrder = new Mock<IReviewRepository>();
            mockOrder.Setup(x => x.GetRestaurantRating(It.IsAny<int>())).Returns(restaurantRatings.AsQueryable());
            var orderFoodActionObject = new ReviewBusiness(mockOrder.Object);
            var data = orderFoodActionObject.GetRestaurantRating(1);
            Assert.AreEqual(1, data.Count());
            Assert.IsNotNull(data);
        }

      
        [Test]
        public void Getratingsbyid_notfound()
        {
            List<TblRating> restaurantRatings = new List<TblRating>();
           
            var mockOrder = new Mock<IReviewRepository>();
            mockOrder.Setup(x => x.GetRestaurantRating(It.IsAny<int>())).Returns(restaurantRatings.AsQueryable());
            var orderFoodActionObject = new ReviewBusiness(mockOrder.Object);
            var data = orderFoodActionObject.GetRestaurantRating(1);
            Assert.AreEqual(null, data);
            Assert.IsNull(data);
        }
        [Test]
        public void Getratingsbycustomerid()
        {
            List<TblRating> restaurantRatings = new List<TblRating>();
            restaurantRatings.Add(new TblRating()
            {
                TblRestaurantId = 1,
                TblCustomerId = 1,
                Rating = 5,
                Comments = "Not Bad",
            });

            var mockOrder = new Mock<IReviewRepository>();
            mockOrder.Setup(x => x.GetRestaurantRatingByCustomer(It.IsAny<int>())).Returns(restaurantRatings.AsQueryable());
            var orderFoodActionObject = new ReviewBusiness(mockOrder.Object);
            var data = orderFoodActionObject.GetRestaurantRatingByCustomer(1);
            Assert.AreEqual(1, data.Count());
            Assert.IsNotNull(data);
        }
        [Test]
        public void Getratingsbycustomerid_notfound()
        {
            List<TblRating> restaurantRatings = new List<TblRating>();

            var mockOrder = new Mock<IReviewRepository>();
            mockOrder.Setup(x => x.GetRestaurantRatingByCustomer(It.IsAny<int>())).Returns(restaurantRatings.AsQueryable());
            var orderFoodActionObject = new ReviewBusiness(mockOrder.Object);
            var data = orderFoodActionObject.GetRestaurantRatingByCustomer(1);
            Assert.AreEqual(0, data.Count());
            Assert.IsEmpty(data);
        }
    }
}

