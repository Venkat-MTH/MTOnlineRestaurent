using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using LoggingManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MT.OnlineRestaurant.BusinessEntities;
using MT.OnlineRestaurant.BusinessLayer;
using MT.OnlineRestaurant.BusinessLayer.Repository;
using Newtonsoft.Json;

namespace MT.OnlineRestaurant.SearchManagement.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class SearchController : Controller
    {
        private readonly IRestaurantBusiness business_Repo;
        private readonly ILogService loggerService;
        public SearchController(IRestaurantBusiness _business_Repo, ILogService _logService)
        {
            business_Repo = _business_Repo;
            loggerService = _logService;
        }
        [HttpGet]
        [Route("GetAllMenus")]
        public async Task<IActionResult> GetAllMenus(int pageNumber, int pageSize)
        {
            try
            {
                int UserId = (Request.Headers.ContainsKey("CustomerId") ? int.Parse(HttpContext.Request.Headers["CustomerId"]) : 0);
                loggerService.LogMessage("MenuList received at endpoint : api/MenuList : UserID : " + UserId);
                var MenuList = await Task<IQueryable<MenuList>>.Run(() => business_Repo.GetMenuList());
                int count = MenuList.Count();

                // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
                int CurrentPage = pageNumber;

                // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
                int PageSize = pageSize;

                // Display TotalCount to Records to User  
                int TotalCount = count;

                // Calculating Totalpage by Dividing (No of Records / Pagesize)  
                 
                int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

                // Returns List of Customer after applying Paging   
                var items = MenuList.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

                // if CurrentPage is greater than 1 means it has previousPage  
                var previousPage = CurrentPage > 1 ? "Yes" : "No";

                // if TotalPages is greater than CurrentPage means it has nextPage  
                var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

                // Object which we are going to send in header   
                var paginationMetadata = new
                {
                    totalCount = TotalCount,
                    pageSize = PageSize,
                    currentPage = CurrentPage,
                    totalPages = TotalPages,
                    previousPage,
                    nextPage
                };
                // Returing List of Customers Collections  
                if (MenuList.Count() == 0)
                    return NotFound("MenuList is empty");
                return Ok(items);
            }
            catch(Exception ex)
            {
                loggerService.LogException(ex);
                return this.StatusCode((int)HttpStatusCode.InternalServerError, string.Empty);
            }
            
        }
        [HttpGet]
        [Route("ResturantDetail")]
        public RestaurantInformation GetResturantDetail([FromQuery] int RestaurantID)
        {          
            RestaurantInformation resturantInformation = new RestaurantInformation();
            resturantInformation = business_Repo.GetResturantDetails(RestaurantID);
            return resturantInformation;
        }

        [HttpGet]
        [Route("ResturantMenuDetail")]
        public IActionResult GetResturantMenuDetail([FromQuery] int RestaurantID)
        {
            IQueryable<RestaurantMenu> restaurantMenuDetails;
            restaurantMenuDetails = business_Repo.GetRestaurantMenus(RestaurantID);
            if (restaurantMenuDetails != null)
            {
                return this.Ok(restaurantMenuDetails);
            }
            return this.StatusCode((int)HttpStatusCode.InternalServerError, string.Empty);
        }

        [HttpGet]
        [Route("ResturantRating")]
        public IActionResult GetResturantRating([FromQuery] int RestaurantID)
        {
            IQueryable<RestaurantRating> restaurantRatings;
            restaurantRatings = business_Repo.GetRestaurantRating(RestaurantID);
            if (restaurantRatings != null)
            {
                return this.Ok(restaurantRatings);
            }

            return this.StatusCode((int)HttpStatusCode.InternalServerError, string.Empty);
        }

        [HttpPost]
        [Route("ResturantRating")]
        public IActionResult ResturantRating([FromQuery] RestaurantRating restaurantRating)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest();
            }

            business_Repo.RestaurantRating(restaurantRating);

            return this.Ok("Submitted the reviewes");
        }

        [HttpGet]
        [Route("ResturantTable")]
        public IActionResult GetResturantTableDetails([FromQuery] int RestaurantID)
        {
            IQueryable<RestaurantTables> restaurant_TableDetails;
            restaurant_TableDetails = business_Repo.GetTableDetails(RestaurantID);
            if (restaurant_TableDetails != null)
            {
                return this.Ok(restaurant_TableDetails);
            }
            return this.StatusCode((int)HttpStatusCode.InternalServerError, string.Empty);
        }

        [HttpPost]
        [Route("SearchRestaurantBasedOnDistance")]
        public IActionResult SearchRestaurantBasedOnDistance([FromBody] LocationDetails locationDetails)
        {
            IQueryable<RestaurantInformation> restaurantDetails;
            restaurantDetails = business_Repo.SearchRestaurantByLocation(locationDetails);
            if (restaurantDetails != null)
            {
                return this.Ok(restaurantDetails);
            }
            return this.StatusCode((int)HttpStatusCode.InternalServerError, string.Empty);

        }

        [HttpPost]
        [Route("SearchRestaurantBasedOnMenu")]
        public IActionResult SearchRestaurantBasedOnMenu([FromBody] AdditionalFeatureForSearch additionalFeatureForSearch)
        {
            IQueryable<RestaurantInformation> restaurantDetails;
            restaurantDetails = business_Repo.GetRestaurantsBasedOnMenu(additionalFeatureForSearch);
            if (restaurantDetails != null)
            {
                return this.Ok(restaurantDetails);
            }
            return this.StatusCode((int)HttpStatusCode.InternalServerError, string.Empty);

        }

        [HttpPost]
        [Route("SearchForRestaurant")]
        public IActionResult SearchForRestaurant([FromBody] SearchForRestaurant searchDetails)
        {
            try
            {
                int UserId = (Request.Headers.ContainsKey("CustomerId") ? int.Parse(HttpContext.Request.Headers["CustomerId"]) : 0);
                loggerService.LogMessage("Received at endpoint : api/SearchForRestaurant : UserID : " + UserId);
                IQueryable<RestaurantInformation> restaurantDetails;
                restaurantDetails = business_Repo.SearchForRestaurant(searchDetails);
                if (restaurantDetails != null)
                {
                    return this.Ok(restaurantDetails);
                }
                return this.StatusCode((int)HttpStatusCode.InternalServerError, string.Empty);
            }
            catch(Exception ex)
            {
                loggerService.LogException(ex);
                return this.StatusCode((int)HttpStatusCode.InternalServerError, string.Empty);
            }

        }
        [HttpGet]
        [Route("OrderDetail")]
        public IActionResult OrderDetail([FromQuery]int restaurantID,int menuID)
        {
            int query_result = business_Repo.ItemInStock(restaurantID, menuID);
            if (query_result > 0)
            {
                return Ok(restaurantID);
            }
            return this.StatusCode((int)HttpStatusCode.InternalServerError, "error");
        }
        [HttpGet]
        [Route("OrderDetails")]
        public IActionResult OrderDetails([FromQuery] string orderedmenuitems)
        {
            loggerService.LogMessage("Received at endpoint : api/OrderDetails : UserID : " + (Request.Headers.ContainsKey("CustomerId") ? int.Parse(HttpContext.Request.Headers["CustomerId"]) : 0));
            var menuitems = orderedmenuitems.Replace("MenuId", "menu_ID");
            var items = JsonConvert.DeserializeObject<List<RestaurantMenu>>(menuitems);
            foreach (var item in items)
            {
                int Menuquantity = business_Repo.ItemInStock(item.menu_ID);
                if (item.quantity<=Menuquantity)
                {
                }
                else
                {
                    if (Menuquantity == 0)
                    {
                        item.quantity = -1;
                    }
                    else
                    {
                        item.quantity = 0;
                    }
                }
            }
            
            return Ok(items);
            //int query_result = business_Repo.ItemInStock(restaurantID, menuID);
            //if (query_result > 0)
            //{
            //    return Ok(restaurantID);
            //}
            //return this.StatusCode((int)HttpStatusCode.InternalServerError, "error");
        }
        [HttpGet]
        [Route("OfferForMenu")]
        public IActionResult OfferForMenu([FromQuery] string orderedmenuitems)
        {
            loggerService.LogMessage("Received at endpoint : api/OfferForMenu : UserID : " + (Request.Headers.ContainsKey("CustomerId") ? int.Parse(HttpContext.Request.Headers["CustomerId"]) : 0));
            bool flag = true;
            var menuitems = orderedmenuitems.Replace("MenuId", "menu_ID");
            var items = JsonConvert.DeserializeObject<List<RestaurantMenu>>(menuitems);
           
            foreach (var item in items)
            {
               bool status = business_Repo.Validateoffer(item.menu_ID,item.offer);
                if (status == false)
                {
                    flag = false;
                }
            }
            if (flag == true)
            {
                return Ok(items);
            }
            else
            {
                return this.StatusCode((int)HttpStatusCode.InternalServerError, "Offer Expired");
            }
           
            //int query_result = business_Repo.ItemInStock(restaurantID, menuID);
            //if (query_result > 0)
            //{
            //    return Ok(restaurantID);
            //}
            //return this.StatusCode((int)HttpStatusCode.InternalServerError, "error");
        }
    }
}