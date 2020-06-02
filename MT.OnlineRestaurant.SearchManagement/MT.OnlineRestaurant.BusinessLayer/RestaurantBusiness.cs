using MT.OnlineRestaurant.BusinessEntities;
using System;
using System.Collections.Generic;
using MT.OnlineRestaurant.DataLayer.Repository;
using MT.OnlineRestaurant.DataLayer.EntityFrameWorkModel;
using MT.OnlineRestaurant.DataLayer.DataEntity;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using MessagesManagement;

namespace MT.OnlineRestaurant.BusinessLayer
{
    public class RestaurantBusiness : IRestaurantBusiness
    {
        ISearchRepository search_Repository;
        private readonly string connectionstring;
        public RestaurantBusiness(ISearchRepository _searchRepository)
        {
            search_Repository = _searchRepository;
        }

        public IQueryable<RestaurantMenu> GetRestaurantMenus(int restaurantID)
        {
            IQueryable<MenuDetails> menuDetails;
            List<RestaurantMenu> restaurant_Menu = new List<RestaurantMenu>();
            try
            {
                menuDetails = search_Repository.GetRestaurantMenu(restaurantID);
                foreach (var menu in menuDetails)
                {
                    RestaurantMenu menuInfo = new RestaurantMenu
                    {
                        menu_ID = menu.tbl_Offer.Id,
                        dish_Name = menu.tbl_Menu.Item,
                        price = menu.tbl_Offer.Price,
                        running_Offer = menu.tbl_Offer.Discount,
                        cuisine = menu.tbl_Cuisine.Cuisine
                    };
                    restaurant_Menu.Add(menuInfo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return restaurant_Menu.AsQueryable();
        }

        public IQueryable<RestaurantRating> GetRestaurantRating(int restaurantID)
        {
            try
            {
                List<RestaurantRating> restaurantRatings = new List<RestaurantRating>();
                IQueryable<TblRating> rating;
                rating = search_Repository.GetRestaurantRating(restaurantID);
                foreach (var item in rating)
                {
                    RestaurantRating ratings = new RestaurantRating
                    {
                        rating = item.Rating,
                        RestaurantId = item.TblRestaurantId,
                        user_Comments = item.Comments,
                        customerId = item.TblCustomerId,
                    };
                    restaurantRatings.Add(ratings);
                }
                return restaurantRatings.AsQueryable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public RestaurantInformation GetResturantDetails(int restaurantID)
        {
            try
            {
                TblRestaurant restaurant = new TblRestaurant();
                restaurant = search_Repository.GetResturantDetails(restaurantID);
                RestaurantInformation resturant_Information = new RestaurantInformation
                {
                    restaurant_ID = restaurant.Id,
                    restaurant_Name = restaurant.Name,
                    restaurant_Address = restaurant.Address,
                    restaurant_ContactNo = restaurant.ContactNo,
                    closing_Time = restaurant.CloseTime,
                    opening_Time = restaurant.OpeningTime,
                    website = restaurant.Website,
                    xaxis = (double)restaurant.TblLocation.X,
                    yaxis = (double)restaurant.TblLocation.Y
                };
                return resturant_Information;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<RestaurantTables> GetTableDetails(int restaurantID)
        {
            try
            {
                List<RestaurantTables> TableInfo = new List<RestaurantTables>();
                IQueryable<TblRestaurantDetails> restaurantTableCount;
                restaurantTableCount = search_Repository.GetTableDetails(restaurantID);
                foreach (var item in restaurantTableCount)
                {
                    RestaurantTables table = new RestaurantTables
                    {
                        restaurant_Name = item.TblRestaurant.Name,
                        table_Capacity = item.TableCapacity,
                        total_Count = item.TableCount
                    };
                    TableInfo.Add(table);
                }
                return TableInfo.AsQueryable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<RestaurantInformation> SearchRestaurantByLocation(BusinessEntities.LocationDetails locationDetails)
        {
            try
            {
                List<RestaurantInformation> restaurant_Info = new List<RestaurantInformation>();
                IQueryable<RestaurantSearchDetails> searched_Restaurant;
                DataLayer.DataEntity.LocationDetails location_Details = new DataLayer.DataEntity.LocationDetails
                {
                    distance = locationDetails.distance,
                    restaurant_Name = locationDetails.restaurant_Name,
                    xaxis = locationDetails.xaxis,
                    yaxis = locationDetails.yaxis
                };

                searched_Restaurant = search_Repository.GetRestaurantsBasedOnLocation(location_Details);
                foreach (var restaurants in searched_Restaurant)
                {
                    RestaurantInformation restaurant_Details = new RestaurantInformation
                    {
                        restaurant_ID = restaurants.restauran_ID,
                        restaurant_Name = restaurants.restaurant_Name,
                        restaurant_Address = restaurants.restaurant_Address,
                        restaurant_ContactNo = restaurants.restaurant_PhoneNumber,
                        closing_Time = restaurants.closing_Time,
                        opening_Time = restaurants.opening_Time,
                        website = restaurants.restraurant_Website,
                        xaxis = restaurants.xaxis,
                        yaxis = restaurants.yaxis
                    };
                    restaurant_Info.Add(restaurant_Details);
                }
                return restaurant_Info.AsQueryable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<RestaurantInformation> GetRestaurantsBasedOnMenu(AdditionalFeatureForSearch additionalFeatureForSearch)
        {
            try
            {
                List<RestaurantInformation> restaurant_Info = new List<RestaurantInformation>();
                IQueryable<RestaurantSearchDetails> searched_Restaurant;
                DataLayer.DataEntity.AddtitionalFeatureForSearch searchCritera = new DataLayer.DataEntity.AddtitionalFeatureForSearch
                {
                    cuisine = (string.IsNullOrEmpty(additionalFeatureForSearch.cuisine) ? "" : additionalFeatureForSearch.cuisine),
                    Menu = (string.IsNullOrEmpty(additionalFeatureForSearch.Menu) ? "" : additionalFeatureForSearch.Menu),
                    rating = (additionalFeatureForSearch.rating != null && additionalFeatureForSearch.rating > 0) ? additionalFeatureForSearch.rating : 0
                };

                searched_Restaurant = search_Repository.GetRestaurantsBasedOnMenu(searchCritera);
                if (searched_Restaurant != null)
                {
                    foreach (var restaurants in searched_Restaurant)
                    {
                        RestaurantInformation restaurant_Details = new RestaurantInformation
                        {
                            restaurant_ID = restaurants.restauran_ID,
                            restaurant_Name = restaurants.restaurant_Name,
                            restaurant_Address = restaurants.restaurant_Address,
                            restaurant_ContactNo = restaurants.restaurant_PhoneNumber,
                            closing_Time = restaurants.closing_Time,
                            opening_Time = restaurants.opening_Time,
                            website = restaurants.restraurant_Website,
                            xaxis = restaurants.xaxis,
                            yaxis = restaurants.yaxis
                        };
                        restaurant_Info.Add(restaurant_Details);
                    }
                }
                return restaurant_Info.AsQueryable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<RestaurantInformation> SearchForRestaurant(SearchForRestaurant searchDetails)
        {
            try
            {
                List<RestaurantInformation> restaurant_Info = new List<RestaurantInformation>();
                IQueryable<RestaurantSearchDetails> searched_Restaurant;
                DataLayer.DataEntity.AddtitionalFeatureForSearch additionalsearch = new DataLayer.DataEntity.AddtitionalFeatureForSearch
                {
                    cuisine = (string.IsNullOrEmpty(searchDetails.search.cuisine) ? "" : searchDetails.search.cuisine),
                    Menu = (string.IsNullOrEmpty(searchDetails.search.Menu) ? "" : searchDetails.search.Menu),
                    rating = (searchDetails.search.rating != null && searchDetails.search.rating > 0) ? searchDetails.search.rating : 0
                };

                DataLayer.DataEntity.LocationDetails locationsearch = new DataLayer.DataEntity.LocationDetails
                {
                    distance = searchDetails.location.distance,
                    restaurant_Name = (string.IsNullOrEmpty(searchDetails.location.restaurant_Name) ? "" : searchDetails.location.restaurant_Name),
                    xaxis = searchDetails.location.xaxis,
                    yaxis = searchDetails.location.yaxis
                };

                DataLayer.DataEntity.SearchForRestautrant searchCritera = new DataLayer.DataEntity.SearchForRestautrant
                {
                    search = additionalsearch,
                    location = locationsearch
                };

                searched_Restaurant = search_Repository.SearchForRestaurant(searchCritera);
                if (searched_Restaurant != null)
                {
                    foreach (var restaurants in searched_Restaurant)
                    {
                        RestaurantInformation restaurant_Details = new RestaurantInformation
                        {
                            restaurant_ID = restaurants.restauran_ID,
                            restaurant_Name = restaurants.restaurant_Name,
                            restaurant_Address = restaurants.restaurant_Address,
                            restaurant_ContactNo = restaurants.restaurant_PhoneNumber,
                            closing_Time = restaurants.closing_Time,
                            opening_Time = restaurants.opening_Time,
                            website = restaurants.restraurant_Website,
                            xaxis = restaurants.xaxis,
                            yaxis = restaurants.yaxis
                        };
                        restaurant_Info.Add(restaurant_Details);
                    }
                }
                return restaurant_Info.AsQueryable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Recording the customer rating the restaurants
        /// </summary>
        /// <param name=""></param>
        public void RestaurantRating(RestaurantRating restaurantRating)
        {
            if (restaurantRating != null)
            {
                TblRating rating = new TblRating()
                {
                    Rating = restaurantRating.rating,
                    TblRestaurantId = restaurantRating.RestaurantId,
                    Comments = restaurantRating.user_Comments,
                    TblCustomerId = restaurantRating.customerId
                };

                search_Repository.RestaurantRating(rating);
            }
        }
        public int ItemInStock(int restaurantID,int menuID)
        {
            RestaurantMenu menuObj = new RestaurantMenu();
            TblMenu menu = search_Repository.ItemInStock(restaurantID,menuID);
            menuObj.quantity = menu.quantity;           
             return 0;
        }
        public int ItemInStock(int menuID)
        {
            RestaurantMenu menuObj = new RestaurantMenu();
            TblMenu menu = search_Repository.ItemInStock(menuID);
            menuObj.quantity = menu.quantity;
            menuObj.price = menu.price;
            return menuObj.quantity;
        }

        public bool Validateoffer(int menuID,bool offer)
        {
            bool res = search_Repository.ValidateOffer(menuID,offer);
            return res;
        }

        public void UpdateItemsinstock(string msg)
        {
            List<OrderMenus> orders = new List<OrderMenus>();
            List<OrderMenus> msgs = JsonConvert.DeserializeObject<List<OrderMenus>>(msg);
            foreach(var item in msgs)
            {
                int updatedquantity = search_Repository.Updateitemstock(item.quantity, item.MenuId);
                if (updatedquantity <= 0)
                {
                    orders.Add(item);
                }
                if (orders.Count >= 1)
                {
                    var items = JsonConvert.SerializeObject(orders);
                    SendMessage.SendMessagesAsync(items);
                    orders.Clear();
                }
            }
        }

        public IQueryable<MenuList> GetMenuList()
        {
            List<MenuList> mlist =  new List<MenuList>();
            IQueryable<TblMenu> tblmenu = search_Repository.MenuDetails();
            foreach(var item in tblmenu)
            {
                MenuList m = new MenuList();
                m.Id = item.Id;
                m.Item = item.Item;
                mlist.Add(m);
            }
            return mlist.AsQueryable();
        }
    }
}
