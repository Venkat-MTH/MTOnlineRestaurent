using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MT.OnlineRestaurant.DataLayer.Context;
using MT.OnlineRestaurant.DataLayer.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#region namespace
namespace MT.OnlineRestaurant.DataLayer
{
    public class CartRepository : ICartRepository
    {
        private readonly OrderManagementContext _context;

        public CartRepository(OrderManagementContext context)
        {
            _context = context;
        }
        #endregion
        public int AddCart(TblCart tblcart)
        {
            if (tblcart != null)
            {
                
                List<TblCart> cart = _context.TblCart.Where(ca => ca.TblCustomerID == tblcart.TblCustomerID).ToList<TblCart>();
                if (cart.Count()>0)
                {
                    foreach (var item in cart)
                    {
                        if (tblcart.TblRestaurantID != item.TblRestaurantID)
                        {
                            _context.TblCart.RemoveRange(_context.TblCart.Where(c => c.TblCustomerID == tblcart.TblCustomerID));
                            tblcart.status = true;
                            tblcart.UserCreated = tblcart.TblCustomerID.HasValue ? tblcart.TblCustomerID.Value : 1;
                            tblcart.UserModified = tblcart.TblCustomerID.HasValue ? tblcart.TblCustomerID.Value : 1;
                            _context.Set<TblCart>().Add(tblcart);
                            _context.SaveChanges();
                            return 1;
                        }
                        if (tblcart.TblMenuID == item.TblMenuID)
                        {
                            item.TblCustomerID = tblcart.TblCustomerID;
                            item.TblRestaurantID = tblcart.TblRestaurantID;
                            item.TblMenuID = tblcart.TblMenuID;
                            item.Price = tblcart.Price;
                            item.Quantity = tblcart.Quantity;
                            item.status = tblcart.status;
                            item.RecordTimeStampCreated = DateTime.Now;
                            item.offer = tblcart.offer;
                            item.RecordTimeStamp = DateTime.Now;
                            _context.SaveChanges();
                            return 1;
                        }
                        if (tblcart.TblRestaurantID == item.TblRestaurantID && tblcart.TblMenuID != item.TblMenuID)
                        {
                            tblcart.status = true;
                            tblcart.UserCreated = tblcart.TblCustomerID.HasValue ? tblcart.TblCustomerID.Value : 1;
                            tblcart.UserModified = tblcart.TblCustomerID.HasValue ? tblcart.TblCustomerID.Value : 1;
                            _context.Set<TblCart>().Add(tblcart);
                            _context.SaveChanges();
                            return 1;
                        }
                    }
                }
                else
                {
                    tblcart.status = true;
                    tblcart.UserCreated = tblcart.TblCustomerID.HasValue ? tblcart.TblCustomerID.Value : 1;
                    tblcart.UserModified = tblcart.TblCustomerID.HasValue ? tblcart.TblCustomerID.Value : 1;
                    _context.Set<TblCart>().Add(tblcart);
                    _context.SaveChanges();
                    return 1;
                }
                

                //if (cart != null)
                //{
                //    if (cart.TblRestaurantID != tblcart.TblRestaurantID )
                //    {
                //        _context.TblCart.RemoveRange(_context.TblCart.Where(c => c.TblCustomerID == tblcart.TblCustomerID));
                //        //List<TblCart> removecartlist = _context.TblCart.Where(d => d.TblCustomerID == tblcart.TblCustomerID).ToList<TblCart>();
                //        //_context.Remove(removecartlist);
                //        //_context.SaveChanges();
                //        tblcart.status = true;
                //        tblcart.UserCreated = tblcart.TblCustomerID.HasValue ? tblcart.TblCustomerID.Value : 1;
                //        tblcart.UserModified = tblcart.TblCustomerID.HasValue ? tblcart.TblCustomerID.Value : 1;
                //        _context.Set<TblCart>().Add(tblcart);
                //        _context.SaveChanges();
                //        return 1;
                //    }
                //    else if (cart.TblRestaurantID == tblcart.TblRestaurantID && cart.TblMenuID == tblcart.TblMenuID) 
                //    {
                //        cart.TblCustomerID = tblcart.TblCustomerID;
                //        cart.TblRestaurantID = tblcart.TblRestaurantID;
                //        cart.TblMenuID = tblcart.TblMenuID;
                //        cart.Price = tblcart.Price;
                //        cart.Quantity = tblcart.Quantity;
                //        cart.status = tblcart.status;
                //        cart.RecordTimeStampCreated = DateTime.Now;
                //        cart.offer = tblcart.offer;
                //        cart.RecordTimeStamp = DateTime.Now;
                //        _context.SaveChanges();
                //        return 1;
                //    }
                //    else
                //    {
                //        tblcart.status = true;
                //        tblcart.UserCreated = tblcart.TblCustomerID.HasValue ? tblcart.TblCustomerID.Value : 1;
                //        tblcart.UserModified = tblcart.TblCustomerID.HasValue ? tblcart.TblCustomerID.Value : 1;
                //        _context.Set<TblCart>().Add(tblcart);
                //        _context.SaveChanges();
                //        return 1;
                //    }
                //}
                //else
                //{
                //    tblcart.status = true;
                //    tblcart.UserCreated = tblcart.TblCustomerID.HasValue ? tblcart.TblCustomerID.Value : 1;
                //    tblcart.UserModified = tblcart.TblCustomerID.HasValue ? tblcart.TblCustomerID.Value : 1;
                //    _context.Set<TblCart>().Add(tblcart);
                //    _context.SaveChanges();
                //    return 1;
                //}

                

            }

            return 0;
        }

        

        public IQueryable<TblCart> GetCartDetails(int CustomerID)
        {
            return _context.TblCart.Where(ca => ca.TblCustomerID == CustomerID).AsQueryable();
        }

        public int RemoveCart(int customerID, int? menuID)
        {
            TblCart tblcart = _context.TblCart.Where(ca => ca.TblCustomerID == customerID)
                .Where(m=>m.TblMenuID==menuID).FirstOrDefault();
            if (tblcart != null)
            {
                _context.Set<TblCart>().Remove(tblcart);
                _context.SaveChanges();
                return 1;
            }
            return 0;
        }

        public int UpdateCart(int CartID)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateCartMenuItemPrice(TblCart tblcart)
        {
            if (tblcart != null)
            {

                List<TblCart> cart = await _context.TblCart.Where(ca => ca.TblMenuID == tblcart.TblMenuID).ToListAsync<TblCart>();

                if (cart.Count != 0)
                {
                    foreach (var cartitem in cart)
                    {
                        cartitem.Price = tblcart.Price;
                        //cartitem.status = tblcart.status; price updated
                        _context.SaveChanges();
                    }
                    
                }
            }
            
        }

        public void UpdateCartitemstatus(TblCart tblcart)
        {
            if (tblcart != null)
            {

                List<TblCart> cart = _context.TblCart.Where(ca => ca.TblMenuID == tblcart.TblMenuID).ToList<TblCart>();

                if (cart.Count != 0)
                {
                    foreach (var cartitem in cart)
                    {
                        cartitem.Price = tblcart.Price;
                        cartitem.status = tblcart.status;
                        cartitem.Itemavailabilitystatus = tblcart.Itemavailabilitystatus;
                        _context.SaveChanges();
                    }

                }
            }
        }

        public async Task UpdateItemoutofstock(int? menuId)
        {
             
                List<TblCart> cart = await _context.TblCart.Where(ca => ca.TblMenuID == menuId).ToListAsync<TblCart>();

                if (cart.Count != 0)
                {
                    foreach (var cartitem in cart)
                    {
                        cartitem.status = false;
                    cartitem.Itemavailabilitystatus = "out of stock";
                        _context.SaveChanges();
                    }

                }
            }
        }
}
