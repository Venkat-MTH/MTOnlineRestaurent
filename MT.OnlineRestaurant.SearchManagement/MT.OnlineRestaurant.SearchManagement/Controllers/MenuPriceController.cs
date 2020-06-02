using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessagesManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MT.OnlineRestaurant.BusinessLayer.Repository;
using MT.OnlineRestaurant.DataLayer.EntityFrameWorkModel;
using Newtonsoft.Json;

namespace MT.OnlineRestaurant.SearchManagement.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class MenuPriceController : Controller
    {
        private readonly IMenuPriceBusiness business_Repo;
        public MenuPriceController(IMenuPriceBusiness _business_Repo)
        {
            business_Repo = _business_Repo;
        }
        [HttpGet]
        [Route("MenuPriceUpdate")]
        public async Task<IActionResult> MenuPriceUpdate([FromQuery] int menuID , int price)
        {
            TblMenu tblmenu = new TblMenu();
            tblmenu = business_Repo.MenuPriceUpdate(menuID,price);
            if (tblmenu != null)
            {
                var pricechange = JsonConvert.SerializeObject(tblmenu);
                await ItemPriceMessage.SendMessagesAsync(pricechange);
                return Ok("Item Price Updated");
            }
            return BadRequest("Failed to price Updated, Please try again later");

        }
    }
}