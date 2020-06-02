using LoggingManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MT.OnlineRestaurant.BusinessEntities;
using MT.OnlineRestaurant.BusinessLayer;
using MT.OnlineRestaurant.ReviewManagement.Controllers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MT.OnlineRestaurant.UT.Controller
{
    [TestFixture]
    class ReviewControllerTests
    {
        [Test]
        public void GetRestaurantRating()
        {
            //Arrange
            List<RestaurantRating> restaurantRatings = new List<RestaurantRating>();
            restaurantRatings.Add(new RestaurantRating()
            {
                RestaurantId = 1,
                customerId = 1,
                rating = "2",
                user_Comments = "Not Bad",
            });
            var mockReview = new Mock<IReviewBusiness>();
            var mockLog = new Mock<ILogService>();
            mockReview.Setup(x => x.GetRestaurantRating(It.IsAny<int>())).Returns(restaurantRatings.AsQueryable());
            mockLog.Setup(x => x.LogMessage("GetRestaurentRating Test case"));
            //Act
            var reviewcontroller = new ReviewController(mockReview.Object, mockLog.Object);
            reviewcontroller.ControllerContext = new ControllerContext();
            reviewcontroller.ControllerContext.HttpContext = new DefaultHttpContext();
            reviewcontroller.ControllerContext.HttpContext.Request.Headers["CustomerId"] = "1";
            var data = reviewcontroller.GetResturantRating(1);
            var okObjectResult = data as OkObjectResult;

            //Assert
            Assert.AreEqual(200, okObjectResult.StatusCode);
            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual((okObjectResult.Value as IEnumerable<RestaurantRating>).Count(), restaurantRatings.Count());
        }
        [Test]
        public void NotFoundRestaurantRatingbyID()
        {
            //Arrange
            List<RestaurantRating> restaurantRatings = new List<RestaurantRating>();
            var mockReview = new Mock<IReviewBusiness>();
            var mockLog = new Mock<ILogService>();
            mockReview.Setup(x => x.GetRestaurantRating(It.IsAny<int>())).Returns(restaurantRatings.AsQueryable());
            mockLog.Setup(x => x.LogMessage("GetRestaurentRating Test case"));
            //Act
            var reviewcontroller = new ReviewController(mockReview.Object, mockLog.Object);
            reviewcontroller.ControllerContext = new ControllerContext();
            reviewcontroller.ControllerContext.HttpContext = new DefaultHttpContext();
            reviewcontroller.ControllerContext.HttpContext.Request.Headers["CustomerId"] = "1";
            var data = reviewcontroller.GetResturantRating(2);
            var notFoundObjectResult = data as NotFoundObjectResult;

            //Assert
            Assert.AreEqual(404, notFoundObjectResult.StatusCode);
        }

        [Test]
        public void ExceptionRestaurantRatingbyID()
        {
            //Arrange
            List<RestaurantRating> restaurantRatings = new List<RestaurantRating>();
            var mockReview = new Mock<IReviewBusiness>();
            var mockLog = new Mock<ILogService>();
            mockReview.Setup(x => x.GetRestaurantRating(It.IsAny<int>())).Returns(restaurantRatings.AsQueryable());
            mockLog.Setup(x => x.LogMessage("GetRestaurentRating Test case"));
            //Act
            var reviewcontroller = new ReviewController(mockReview.Object, mockLog.Object);
            reviewcontroller.ControllerContext = new ControllerContext();
            reviewcontroller.ControllerContext.HttpContext = new DefaultHttpContext();
            reviewcontroller.ControllerContext.HttpContext.Request.Headers["CustomerId"] = "aa";
            var data = reviewcontroller.GetResturantRating(2);
            var exObjectResult = data as ObjectResult;

            //Assert
            Assert.AreEqual(500, exObjectResult.StatusCode);
        }



        [Test]
        public void NotFoundRestaurantRatingbyCustomerID()
        {
            //Arrange
            List<RestaurantRating> restaurantRatings = new List<RestaurantRating>();
            var mockReview = new Mock<IReviewBusiness>();
            var mockLog = new Mock<ILogService>();
            mockReview.Setup(x => x.GetRestaurantRatingByCustomer(It.IsAny<int>())).Returns(restaurantRatings.AsQueryable());
            mockLog.Setup(x => x.LogMessage("GetRestaurentRating Test case"));
            //Act
            var reviewcontroller = new ReviewController(mockReview.Object, mockLog.Object);
            reviewcontroller.ControllerContext = new ControllerContext();
            reviewcontroller.ControllerContext.HttpContext = new DefaultHttpContext();
            reviewcontroller.ControllerContext.HttpContext.Request.Headers["CustomerId"] = "1";
            var data = reviewcontroller.GetResturantRatingByCustomerID(2);
            var notFoundObjectResult = data as NotFoundObjectResult;

            //Assert
            Assert.AreEqual(404, notFoundObjectResult.StatusCode);
        }
       
        [Test]
        public void GetRestaurantRatingByCustomerID()
        {
            //Arrange
            List<RestaurantRating> restaurantRatings = new List<RestaurantRating>();
            restaurantRatings.Add(new RestaurantRating()
            {
                RestaurantId = 1,
                customerId = 1,
                rating = "5",
                user_Comments = "Good",
            });
            var mockOrder = new Mock<IReviewBusiness>();
            var mockOrder1 = new Mock<ILogService>();
            mockOrder.Setup(x => x.GetRestaurantRatingByCustomer(It.IsAny<int>())).Returns(restaurantRatings.AsQueryable());
            mockOrder1.Setup(x => x.LogMessage("GetRestaurentRatingbyCustomerID Test case"));
            //Act
            var reviewcontroller = new ReviewController(mockOrder.Object, mockOrder1.Object);
            reviewcontroller.ControllerContext = new ControllerContext();
            reviewcontroller.ControllerContext.HttpContext = new DefaultHttpContext();
            reviewcontroller.ControllerContext.HttpContext.Request.Headers["CustomerId"] = "1";
            var data = reviewcontroller.GetResturantRatingByCustomerID(1);
            var okObjectResult = data as OkObjectResult;

            //Assert
            Assert.AreEqual(200, okObjectResult.StatusCode);
            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual((okObjectResult.Value as IEnumerable<RestaurantRating>).Count(), restaurantRatings.Count());
        }

        [Test]
        public void ExceptionRestaurantRatingByCustomerID()
        {
            //Arrange
            List<RestaurantRating> restaurantRatings = new List<RestaurantRating>();
            restaurantRatings.Add(new RestaurantRating()
            {
                RestaurantId = 1,
                customerId = 1,
                rating = "5",
                user_Comments = "Good",
            });
            var mockOrder = new Mock<IReviewBusiness>();
            var mockOrder1 = new Mock<ILogService>();
            mockOrder.Setup(x => x.GetRestaurantRatingByCustomer(It.IsAny<int>())).Returns(restaurantRatings.AsQueryable());
            mockOrder1.Setup(x => x.LogMessage("GetRestaurentRatingbyCustomerID Test case"));
            //Act
            var reviewcontroller = new ReviewController(mockOrder.Object, mockOrder1.Object);
            reviewcontroller.ControllerContext = new ControllerContext();
            reviewcontroller.ControllerContext.HttpContext = new DefaultHttpContext();
            reviewcontroller.ControllerContext.HttpContext.Request.Headers["CustomerId"] = "abc";
            var data = reviewcontroller.GetResturantRatingByCustomerID(2);
            var exceptionObjectResult = data as ObjectResult;

            //Assert
            Assert.AreEqual(500, exceptionObjectResult.StatusCode);
            
        }
        [Test]
        public void SubmitReview()
        {
            //Arrange
            RestaurantRating restaurantRatings = new RestaurantRating()
            {
                RestaurantId = 1,
                customerId = 1,
                rating = "8",
                user_Comments = "Excellent",
            };
            var mockOrder = new Mock<IReviewBusiness>();
            var mockOrder1 = new Mock<ILogService>();
            mockOrder.Setup(x => x.RestaurantRating(It.IsAny<RestaurantRating>()));
            mockOrder1.Setup(x => x.LogMessage("Submitted Reviews TestCase"));
            //Act
            var reviewcontroller = new ReviewController(mockOrder.Object, mockOrder1.Object);
            reviewcontroller.ControllerContext = new ControllerContext();
            reviewcontroller.ControllerContext.HttpContext = new DefaultHttpContext();
            reviewcontroller.ControllerContext.HttpContext.Request.Headers["CustomerId"] = "1";
            var data = reviewcontroller.ResturantRating(restaurantRatings);
            var okObjectResult = data as OkObjectResult;

            //Assert
            Assert.AreEqual(200, okObjectResult.StatusCode);
            Assert.IsNotNull(okObjectResult);
        }
        [Test]
        public void InvalidSubmitReview()
        {
            //Arrange
            RestaurantRating restaurantRatings = new RestaurantRating();
            restaurantRatings = null;
            var mockOrder = new Mock<IReviewBusiness>();
            var mockOrder1 = new Mock<ILogService>();
            mockOrder.Setup(x => x.RestaurantRating(It.IsAny<RestaurantRating>()));
            mockOrder1.Setup(x => x.LogMessage("Invalid Submitted Reviews TestCase"));
            //Act
            var reviewcontroller = new ReviewController(mockOrder.Object, mockOrder1.Object);
            
            reviewcontroller.ControllerContext = new ControllerContext();
            reviewcontroller.ControllerContext.HttpContext = new DefaultHttpContext();
            reviewcontroller.ControllerContext.HttpContext.Request.Headers["CustomerId"] = "1";
            reviewcontroller.ModelState.AddModelError("fakeError", "fakeError");
            var data = reviewcontroller.ResturantRating(restaurantRatings);
            var badObjectResult = data as BadRequestResult;

            //Assert
            Assert.AreEqual(400, badObjectResult.StatusCode);
            Assert.IsNull(restaurantRatings);
        }
        [Test]
        public void ExceptionInvalidSubmitReview()
        {
            //Arrange
            RestaurantRating restaurantRatings = new RestaurantRating()
            {
                RestaurantId = 1,
                customerId = 1,
                rating = "8",
                user_Comments = "Excellent",
            };
            var mockOrder = new Mock<IReviewBusiness>();
            var mockOrder1 = new Mock<ILogService>();
            mockOrder.Setup(x => x.RestaurantRating(It.IsAny<RestaurantRating>()));
            mockOrder1.Setup(x => x.LogMessage("Exception in Submitted Reviews TestCase"));
            //Act
            var reviewcontroller = new ReviewController(mockOrder.Object, mockOrder1.Object);

            reviewcontroller.ControllerContext = new ControllerContext();
            reviewcontroller.ControllerContext.HttpContext = new DefaultHttpContext();
            reviewcontroller.ControllerContext.HttpContext.Request.Headers["CustomerId"] = "a";
           // reviewcontroller.ModelState.AddModelError("1", "Invalid Model");
            var data = reviewcontroller.ResturantRating(restaurantRatings);
            var badObjectResult = data as ObjectResult;

            ////Assert
            Assert.AreEqual(500, badObjectResult.StatusCode);
            
            
        }


    }
}
