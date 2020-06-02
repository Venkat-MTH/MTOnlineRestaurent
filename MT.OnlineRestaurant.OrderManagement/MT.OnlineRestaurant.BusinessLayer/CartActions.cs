using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MT.OnlineRestaurant.BusinessEntities;
using MT.OnlineRestaurant.BusinessLayer.interfaces;
using MT.OnlineRestaurant.DataLayer;
using MT.OnlineRestaurant.DataLayer.Context;
using MT.OnlineRestaurant.DataLayer.interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#region namespace
namespace MT.OnlineRestaurant.BusinessLayer
{
    public class CartActions : ICartActions
    {
        private readonly ICartRepository _context;

        public CartActions(ICartRepository context)
        {
            _context = context;
        }
        #endregion
        public int AddCart(CartItemsEntity tblcart)
        {
            if (tblcart != null)
            {
                return _context.AddCart(new DataLayer.Context.TblCart()
                {
                    //Id = tblcart.CartId,
                    TblCustomerID = tblcart.TblCustomerID,
                    TblRestaurantID = tblcart.TblRestaurantID,
                    TblMenuID = tblcart.TblMenuID,
                    Price = tblcart.Price,
                    Quantity = tblcart.Quantity,
                    status = tblcart.Status,
                    offer = tblcart.offer,
                    RecordTimeStampCreated = DateTime.Now,
                    RecordTimeStamp = DateTime.Now
                }); 

            }

            return 0;
        }

        public List<CartItemsEntity> GetCartDetails(int CustomerID)
        {
            var Cartitems = _context.GetCartDetails(CustomerID);

            if (Cartitems.Any())
            {
                return Cartitems.Select(tblcart => new CartItemsEntity
                {
                    //CartId = tblcart.Id,
                    TblCustomerID = tblcart.TblCustomerID,
                    TblRestaurantID = tblcart.TblRestaurantID,
                    TblMenuID = tblcart.TblMenuID,
                    Price = tblcart.Price,
                    offer = tblcart.offer,
                    Quantity = tblcart.Quantity,
                    Status = tblcart.status,
                    RecordTimeStampCreated = DateTime.Now,
                    RecordTimeStamp = DateTime.Now
                }).ToList();
            }
            return null;
        }

        public void UpdateCartitemstatus(CartItemsEntity tblcart)
        {
            if (tblcart != null)
            {
                TblCart tc = new TblCart();
                tc.TblMenuID = tblcart.TblMenuID;
                tc.Price = tblcart.Price;
                tc.Itemavailabilitystatus = tblcart.Itemavailabilitystatus;
                _context.UpdateCartitemstatus(tc);

            }
        }

        public async Task UpdateCartMenuItemPrice(CartItemsEntity tblcart)
        {
             
            if (tblcart != null)
            {
                TblCart tc = new TblCart();
                tc.TblMenuID = tblcart.TblMenuID;
                tc.Price = tblcart.Price;
                tc.Itemavailabilitystatus = tblcart.Itemavailabilitystatus;
                await _context.UpdateCartMenuItemPrice(tc);
                
            }
           
        }

        public async Task UpdateItemoutofstock(string msg)
        {
            List<OrderMenus> orders = new List<OrderMenus>();
            List<OrderMenus> msgs = JsonConvert.DeserializeObject<List<OrderMenus>>(msg);
            foreach (var item in msgs)
            {
               await _context.UpdateItemoutofstock(item.MenuId);
            }
        }
        public void Removecart(int customerID, int? menuID)
        {
            int i = _context.RemoveCart(customerID, menuID);
        }
    }
}
