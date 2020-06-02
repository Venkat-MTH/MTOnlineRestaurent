using AutoMapper;
using Microsoft.Extensions.Options;
using MT.OnlineRestaurant.BusinessEntities;
using MT.OnlineRestaurant.BusinessEntities.ServiceModels;
using MT.OnlineRestaurant.BusinessLayer.interfaces;
using MT.OnlineRestaurant.DataLayer;
using MT.OnlineRestaurant.DataLayer.interfaces;
using MT.OnlineRestaurant.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static Microsoft.Azure.Amqp.Serialization.SerializableType;

namespace MT.OnlineRestaurant.BusinessLayer
{
    public class PlaceOrderActions : IPlaceOrderActions
    {
        // Create a field to store the mapper object
        private readonly IMapper _mapper;
        private readonly IPlaceOrderDbAccess _placeOrderDbAccess;
        private readonly IOptions<ConnectionStrings> _connectionStrings;
        private readonly ICartActions _cartActions;
        public PlaceOrderActions()
        {
            
        }

        public PlaceOrderActions(IPlaceOrderDbAccess placeOrderDbAccess)
        {
            _placeOrderDbAccess = placeOrderDbAccess;
        }

        public PlaceOrderActions(IPlaceOrderDbAccess placeOrderDbAccess, IMapper mapper, IOptions<ConnectionStrings> connectionStrings)
        {
            _placeOrderDbAccess = placeOrderDbAccess;
            _mapper = mapper;
            _connectionStrings = connectionStrings;
        }
        public PlaceOrderActions(IPlaceOrderDbAccess placeOrderDbAccess, IMapper mapper, IOptions<ConnectionStrings> connectionStrings,ICartActions cartactions)
        {
            _placeOrderDbAccess = placeOrderDbAccess;
            _mapper = mapper;
            _connectionStrings = connectionStrings;
            _cartActions = cartactions;
        }

        /// <summary>
        /// Place order
        /// </summary>
        /// <param name="orderEntity">Order details</param>
        /// <returns>order id</returns>
        public int PlaceOrder(OrderEntity orderEntity)
        {
            DataLayer.Context.TblFoodOrder tblFoodOrder = _mapper.Map<DataLayer.Context.TblFoodOrder>(orderEntity);

            IList<DataLayer.Context.TblFoodOrderMapping> tblFoodOrderMappings = new List<DataLayer.Context.TblFoodOrderMapping>();
    
            foreach (OrderMenus orderMenu in orderEntity.OrderMenuDetails)
            {
                tblFoodOrderMappings.Add(new DataLayer.Context.TblFoodOrderMapping()
                {
                    TblFoodOrderId = 0,
                    TblMenuId = orderMenu.MenuId,
                    Price = orderMenu.Price,
                    quantity= orderMenu.quantity,
                    UserCreated = 0,
                    RecordTimeStampCreated = DateTime.Now
                });
            }

            return _placeOrderDbAccess.PlaceOrder(tblFoodOrder);            
        }

        /// <summary>
        /// Cancel Order
        /// </summary>
        /// <param name="orderId">order id</param>
        /// <returns></returns>
        public int CancelOrder(int orderId)
        {
            return (orderId > 0 ? _placeOrderDbAccess.CancelOrder(orderId) : 0);
        }

        /// <summary>
        /// gets the customer placed order details
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public IQueryable<CustomerOrderReport> GetReports(int customerId)
        {
            var foodOrders = _placeOrderDbAccess.GetReports(customerId);
            if (foodOrders.Any())
            {
                return foodOrders.Select(x => new CustomerOrderReport
                {
                    OrderedDate = x.RecordTimeStampCreated,
                    OrderStatus = x.TblOrderStatus.Status,
                    OrderId = x.Id,
                    PaymentStatus = x.TblOrderPayment.Any() ? x.TblOrderPayment.FirstOrDefault().TblPaymentStatus.Status : string.Empty,
                    price = x.TotalPrice
                }).AsQueryable();
            }

            return null;
        }

        public async Task<bool> IsValidRestaurantAsync(OrderEntity orderEntity, int UserId, string UserToken)
        {
            using (HttpClient httpClient = WebAPIClient.GetClient(UserToken, UserId, _connectionStrings.Value.RestaurantApiUrl))
            {
                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync("api/ResturantDetail?RestaurantID=" + orderEntity.RestaurantId);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string json = await httpResponseMessage.Content.ReadAsStringAsync();
                    RestaurantInformation restaurantInformation = JsonConvert.DeserializeObject<RestaurantInformation>(json);
                    if(restaurantInformation != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public async Task<bool> IsOrderItemInStock(OrderEntity orderEntity, int UserId, string UserToken)
        {
            
            bool flag = true;
            using (HttpClient httpClient = WebAPIClient.GetClient(UserToken, UserId, _connectionStrings.Value.RestaurantApiUrl = "https://mtonlinerestaurantsearchmanagement.azurewebsites.net/api/OrderDetails?orderedmenuitems="))
            //using (HttpClient httpClient = new HttpClient())
            {
                var ordermenudetails = JsonConvert.SerializeObject(orderEntity.OrderMenuDetails);
                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(_connectionStrings.Value.RestaurantApiUrl+ ordermenudetails);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string json = await httpResponseMessage.Content.ReadAsStringAsync();
                    List <OrderedMeniItems> ordereditems = JsonConvert.DeserializeObject<List<OrderedMeniItems>>(json);
                    if (ordereditems != null)
                    {
                        foreach(var item in ordereditems)
                        {
                            if (item.quantity == 0)
                            {
                                flag = false;

                                CartItemsEntity cart = new CartItemsEntity()
                                {
                                    Status = false,
                                    TblMenuID = item.menu_ID,
                                    Price = item.price,
                                    Itemavailabilitystatus = "OutofStock",
                                    TblRestaurantID = orderEntity.RestaurantId
                                };

                                 _cartActions.UpdateCartitemstatus(cart);
                            }
                            else if ( item.quantity == -1)
                            {
                                flag = false;

                                CartItemsEntity cart = new CartItemsEntity()
                                {
                                    Status = false,
                                    TblMenuID = item.menu_ID,
                                    Price = item.price,
                                    Itemavailabilitystatus = "Requested Quantity Not Available",
                                    TblRestaurantID = orderEntity.RestaurantId
                                };

                                 _cartActions.UpdateCartitemstatus(cart);
                            }
                        }
                        return flag;
                    }
                }
            }
            return false;
        }

        public async Task<bool> IsValidOfferAsync(OrderEntity orderEntity, int UserId, string UserToken)
        {
            using (HttpClient httpClient = WebAPIClient.GetClient(UserToken, UserId, _connectionStrings.Value.RestaurantApiUrl= "https://mtonlinerestaurantsearchmanagement.azurewebsites.net/api/OfferForMenu?orderedmenuitems="))
            //using (HttpClient httpClient = new HttpClient())
            {
                var val = JsonConvert.SerializeObject(orderEntity.OrderMenuDetails);
                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(_connectionStrings.Value.RestaurantApiUrl + val);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string json = await httpResponseMessage.Content.ReadAsStringAsync();
                    List<OrderedMeniItems> ordereditems = JsonConvert.DeserializeObject<List<OrderedMeniItems>>(json);
                    if (ordereditems != null)
                    {
                        return true;
                    }

                }
            }
            return false;
        }

        //public async Task<bool> IsValidRestaurant(OrderEntity orderEntity)
        //{
        //    using (HttpClient httpClient = WebAPIClient.GetClient(orderEntity.UserToken, orderEntity.UserId, "http://localhost:10601/"))
        //    {
        //        HttpResponseMessage httpResponseMessage = await httpClient.GetAsync("api/ResturantMenuDetail?RestaurantID=" + orderEntity.RestaurantId);
        //        if (httpResponseMessage.IsSuccessStatusCode)
        //        {
        //            string json = await httpResponseMessage.Content.ReadAsStringAsync();
        //            IQueryable<RestaurantMenu> restaurantInformation = JsonConvert.DeserializeObject<IQueryable<RestaurantMenu>>(json);
        //            if (restaurantInformation != null)
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}

        public OrderEntity mappingorderandcartitems(int userid)
        {
            OrderEntity orderEntity = new OrderEntity();
            List<OrderMenus> om1 = new List<OrderMenus>();
            List<CartItemsEntity> cartitems = new List<CartItemsEntity>();
            cartitems = _cartActions.GetCartDetails(userid);
            if (cartitems == null)
            {
                return null;
            }
            foreach (var item in cartitems)
            {
                orderEntity.CustomerId = (int)item.TblCustomerID;
                orderEntity.RestaurantId = (int)item.TblRestaurantID;
                orderEntity.DeliveryAddress = "abc";
                OrderMenus om = new OrderMenus();
                om.MenuId = item.TblMenuID;
                om.Price = item.Price;
                om.offer = item.offer;
                om.quantity = (int)item.Quantity;
                om1.Add(om);
            }
            orderEntity.OrderMenuDetails = new List<OrderMenus>(om1);
            return orderEntity;
        }
    }

    public class OrderedMeniItems
    {
        public int menu_ID { get; set; }
        public decimal price { get; set; }
        public int quantity { get; set; }
    }
}
