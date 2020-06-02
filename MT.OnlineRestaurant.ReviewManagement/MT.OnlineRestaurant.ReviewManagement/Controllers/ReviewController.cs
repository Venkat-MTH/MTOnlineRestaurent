using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LoggingManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MT.OnlineRestaurant.BusinessEntities;
using MT.OnlineRestaurant.BusinessLayer;

namespace MT.OnlineRestaurant.ReviewManagement.Controllers
{
    [Produces("application/json")]
    
    public class ReviewController : Controller
    {
        private readonly IReviewBusiness business_Repo;
        private readonly ILogService logservice;
        public ReviewController(IReviewBusiness _business_Repo , ILogService _logService)
        {
            business_Repo = _business_Repo;
            logservice = _logService;
        }
        [HttpGet]
        [Route("ResturantRating")]
        public IActionResult GetResturantRating([FromQuery] int RestaurantID)
        { 
            try
            {
                int UserId = (Request.Headers.ContainsKey("CustomerId") ? int.Parse(HttpContext.Request.Headers["CustomerId"]) : 0);
                logservice.LogMessage("Get ResturantRating: api/ResturantRating: userID: " + UserId);
                IQueryable<RestaurantRating> restaurantRatings;
                restaurantRatings = business_Repo.GetRestaurantRating(RestaurantID);
                if (restaurantRatings != null && restaurantRatings.Count() > 0)
                {
                    return this.Ok(restaurantRatings);
                }
                return NotFound("The requested review details not found. Please try again with valid restaurant id.");
            }
            catch (Exception ex)
            {
                logservice.LogException(ex);
                return this.StatusCode((int)HttpStatusCode.InternalServerError, string.Empty);
            }
        }
        [HttpGet]
        [Route("ResturantRatingByCustomer")]
        public IActionResult GetResturantRatingByCustomerID([FromQuery] int CustomerID)
        {
            try
            {
                int UserId = (Request.Headers.ContainsKey("CustomerId") ? int.Parse(HttpContext.Request.Headers["CustomerId"]) : 0);
                logservice.LogMessage("GetResturantRatingByCustomerID: api/ResturantRatingByCustomer: userID: " + UserId);
                IQueryable<RestaurantRating> restaurantRatings;
                restaurantRatings = business_Repo.GetRestaurantRatingByCustomer(CustomerID);
                if (restaurantRatings != null && restaurantRatings.Count() > 0)
                {
                    return this.Ok(restaurantRatings);
                }
                return NotFound("The requested review details not found. Please try again with valid Customer id.");
            }
             catch(Exception ex)
            {
                logservice.LogException(ex);
                return this.StatusCode((int)HttpStatusCode.InternalServerError, string.Empty);
            }
        }

        [HttpPost]
        [Route("ResturantRating")]
        public IActionResult ResturantRating([FromBody] RestaurantRating restaurantRating)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest();
            }
            try
            {
              //  int UserId = (Request.Headers.ContainsKey("CustomerId") ? int.Parse(HttpContext.Request.Headers["CustomerId"]) : 0);
                logservice.LogMessage("Submit Resturant Rating: Api: api/ResturantRating: user ID:"+ (Request.Headers.ContainsKey("CustomerId") ? int.Parse(HttpContext.Request.Headers["CustomerId"]) : 0));
                business_Repo.RestaurantRating(restaurantRating);
            }
            catch(Exception ex)
            {
                logservice.LogException(ex);
                return this.StatusCode((int)HttpStatusCode.InternalServerError, string.Empty);
            }
            

            return this.Ok("Submitted the reviewes");
        }
    }
}