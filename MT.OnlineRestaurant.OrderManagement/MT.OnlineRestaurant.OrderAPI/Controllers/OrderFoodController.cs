#region References
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using MT.OnlineRestaurant.BusinessEntities;
using MT.OnlineRestaurant.BusinessLayer.interfaces;
using MT.OnlineRestaurant.OrderAPI.ModelValidators;
using System;
using System.Linq;
using System.Threading.Tasks;

using LoggingManagement;
using Microsoft.Azure.ServiceBus;
using System.Text;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights;
using Newtonsoft.Json;
using MessagesManagement;
using StackExchange.Redis;
using System.Collections;
using System.Collections.Generic;
#endregion

#region Namespace
namespace MT.OnlineRestaurant.OrderAPI.Controllers
{
    /// <summary>
    /// Food ordering controller
    /// </summary>
    [Produces("application/json")]
    public class OrderFoodController : Controller
    {
        private readonly IPlaceOrderActions _placeOrderActions;
        private readonly ILogService _logService;
        private readonly ICartActions _icartActions;
        /// <summary>
        /// Inject buisiness layer dependency
        /// </summary>
        /// <param name="placeOrderActions">Instance of this interface is injected in startup</param>
        public OrderFoodController(IPlaceOrderActions placeOrderActions, ILogService logService , ICartActions cartActions)
        {
            _placeOrderActions = placeOrderActions;
            _logService = logService;
            _icartActions = cartActions;
        }       
        /// <summary>
        /// POST api/OrderFood
        /// To order food
        /// </summary>
        /// <param name="orderEntity">Order entity</param>
        /// <returns>Status of order</returns>
        [HttpPost]
        [Route("api/OrderFood")]
        public async Task<IActionResult> Post([FromBody]OrderEntity orderEntity)
        {
            _logService.LogMessage("Order Entity received at endpoint : api/OrderFood, User ID : "+orderEntity.CustomerId);
            int UserId = (Request.Headers.ContainsKey("CustomerId") ? int.Parse(HttpContext.Request.Headers["CustomerId"]) : 0);
            string UserToken = (Request.Headers.ContainsKey("AuthToken") ? Convert.ToString(HttpContext.Request.Headers["AuthToken"]) : "");

            OrderEntityValidator orderEntityValidator = new OrderEntityValidator(UserId, UserToken, _placeOrderActions);
            ValidationResult validationResult = orderEntityValidator.Validate(orderEntity);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToString("; "));
            }
            else
            {
                var result = await Task<int>.Run(() => _placeOrderActions.PlaceOrder(orderEntity));
                if (result == 0)
                {
                    return BadRequest("Failed to place order, Please try again later");
                }
                else
                {
                    // Publish Quantity of menu to search service.
                    //update cart quantity if menu exists.
                    var pubquantity = JsonConvert.SerializeObject(orderEntity.OrderMenuDetails);
                    await PublishOrderPlaced.SendMessagesAsync(pubquantity);
                    //foreach (var item in orderEntity.OrderMenuDetails)
                    //{
                    //    _icartActions.Removecart(orderEntity.CustomerId, item.MenuId>0?item.MenuId:0);
                    //}
                    
                }
            }
            return Ok("Order placed successfully");
        }

        /// <summary>
        /// POST api/OrderFoodFromCart
        /// To order food from Cart
        /// </summary>
        /// <returns>Status of order</returns>
        [HttpPost]
        [Route("api/OrderFoodFromCart")]
        public async Task<IActionResult> OrderFoodFromCart()
        {
            _logService.LogMessage("Order Entity received at endpoint : api/OrderFoodFromCart");
            int UserId = (Request.Headers.ContainsKey("CustomerId") ? int.Parse(HttpContext.Request.Headers["CustomerId"]) : 0);
            string UserToken = (Request.Headers.ContainsKey("AuthToken") ? Convert.ToString(HttpContext.Request.Headers["AuthToken"]) : "");
            OrderEntity orderEntity = _placeOrderActions.mappingorderandcartitems(UserId);
            if (orderEntity ==null)
            {
                return BadRequest("No Items in Cart");
            }
            ////List<OrderMenus> om1 = new List<OrderMenus>();
            ////List<CartItemsEntity> cartitems = new List<CartItemsEntity>();
            ////cartitems = _icartActions.GetCartDetails(UserId);
            ////foreach(var item in cartitems)
            ////{
            ////    orderEntity.CustomerId = (int) item.TblCustomerID;
            ////    orderEntity.RestaurantId = (int)item.TblRestaurantID;
            ////    orderEntity.DeliveryAddress = "abc";
            ////    OrderMenus om = new OrderMenus();
            ////    om.MenuId = item.TblMenuID;
            ////    om.Price = item.Price;
            ////    om.quantity =(int) item.Quantity;
            ////    om1.Add(om);
            ////}
            //orderEntity.OrderMenuDetails = new List<OrderMenus>(om1);
            else
            {
                OrderEntityValidator orderEntityValidator = new OrderEntityValidator(UserId, UserToken, _placeOrderActions);
                ValidationResult validationResult = orderEntityValidator.Validate(orderEntity);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.ToString("; "));
                }
                else
                {
                    var result = await Task<int>.Run(() => _placeOrderActions.PlaceOrder(orderEntity));
                    if (result == 0)
                    {
                        return BadRequest("Failed to place order, Please try again later");
                    }
                    else
                    {
                        // Publish Quantity of menu to search service.
                        //remove cart.
                        var pub = JsonConvert.SerializeObject(orderEntity.OrderMenuDetails);
                        await PublishOrderPlaced.SendMessagesAsync(pub);
                        foreach (var item in orderEntity.OrderMenuDetails)
                        {
                            _icartActions.Removecart(orderEntity.CustomerId, item.MenuId > 0 ? item.MenuId : 0);
                        }

                    }
                }
            }
            
            return Ok("Order placed successfully");
        }

        /// <summary>
        /// DELETE api/CancelOrder
        /// Cancel the order
        /// </summary>
        /// <param name="id">Order id</param>
        /// <returns>Status of order</returns>
        [HttpDelete]
        [Route("api/CancelOrder")]
        public IActionResult Delete(int id)
        {
            var result = _placeOrderActions.CancelOrder(id);
            if (result > 0)
            {
                return Ok("Order cancelled successfully");
            }

            return BadRequest("Failed to cancel order, Please try again later");
        }      
        //[HttpGet]
        //[Route("api/Reports")]
        //public IActionResult Reports(int customerId)
        //{
        //    IQueryable<CustomerOrderReport> result = _placeOrderActions.GetReports(customerId);
        //    if(result.Any())
        //    {
        //        return Ok(result.ToList());
        //    }

        //    return BadRequest("Failed to get the reports");
        //}     
    }
}
#endregion