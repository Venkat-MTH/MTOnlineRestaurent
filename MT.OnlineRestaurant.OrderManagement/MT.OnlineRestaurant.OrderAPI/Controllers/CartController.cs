using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using LoggingManagement;
using Microsoft.AspNetCore.Mvc;
using MT.Online.Restaurant.MessagesManagement.Services;
using MT.OnlineRestaurant.BusinessEntities;
using MT.OnlineRestaurant.BusinessLayer.interfaces;
using MT.OnlineRestaurant.OrderAPI.ModelValidators;

#region Namespace
namespace MT.OnlineRestaurant.OrderAPI.Controllers
{


    /// <summary>
    /// Cart controller
    /// </summary>
    [Produces("application/json")]
    public class CartController : Controller
    {
        private readonly ICartActions _cartAction;
        private readonly ILogService _logService;
        private readonly IConsumePriceChange _message;
        /// <summary>
        /// Inject buisiness layer dependency
        /// </summary>
        /// <param name="cartAction">Instance of this interface is injected in startup</param>
        /// <param name="logService"></param>
        /// <param name="message"></param>
        public CartController(ICartActions cartAction, ILogService logService, IConsumePriceChange message)
        {
            _cartAction = cartAction;
            _logService = logService;
            _message = message;
        }
        /// <summary>
        /// POST api/UpdateCart
        /// Add or Update Cart
        /// </summary>
        /// <param name="cartItemsEntity">cartItems entity</param>
        /// <returns>Status of Cart</returns>
        [HttpPost]
        [Route("api/AddtoCart")]
        public async Task<IActionResult> Post([FromBody]CartItemsEntity cartItemsEntity)
        {

            _logService.LogMessage("CartItem Entity received at endpoint : api/UpdateCart, User ID : " + cartItemsEntity.TblCustomerID);
            int UserId = (Request.Headers.ContainsKey("CustomerId") ? int.Parse(HttpContext.Request.Headers["CustomerId"]) : 0);
            string UserToken = (Request.Headers.ContainsKey("AuthToken") ? Convert.ToString(HttpContext.Request.Headers["AuthToken"]) : "");

            //OrderEntityValidator cartEntityValidator = new OrderEntityValidator(UserId, UserToken, _placeorderAction);
            //ValidationResult validationResult = cartEntityValidator.Validate(cartItemsEntity);
            //if (!validationResult.IsValid)
            //{
            //    return BadRequest(validationResult.ToString("; "));
            //}
            //else
            //{
                var result = await Task<int>.Run(() => _cartAction.AddCart(cartItemsEntity));
            if (result == 0)
            {
                return BadRequest("Failed to Add cart, Please try again later");
            }
            //}
            return Ok("Add to cart successfully");
        }
    }
}
#endregion