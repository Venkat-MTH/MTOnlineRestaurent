using System;
using System.Collections.Generic;
using System.Text;
using MT.OnlineRestaurant.DataLayer.EntityFrameWorkModel;
using System.Linq;
using MT.OnlineRestaurant.DataLayer.DataEntity;
using Microsoft.Extensions.Options;

namespace MT.OnlineRestaurant.DataLayer.Repository
{
    public class SearchRepository : ISearchRepository
    {
        private readonly RestaurantManagementContext db;
        public SearchRepository(RestaurantManagementContext connection)
        {
            db = connection;
        }
       

        #region Interface Methods
        public IQueryable<MenuDetails> GetRestaurantMenu(int restaurantID)
        {
            List<MenuDetails> menudetails = new List<MenuDetails>();
            try
            {
                if (db != null)
                {
                    var menudetail = (from offer in db.TblOffer
                                      join menu in db.TblMenu
                                      on offer.TblMenuId equals menu.Id into TableMenu
                                      from menu in TableMenu.ToList()
                                      join cuisine in db.TblCuisine on menu.TblCuisineId equals cuisine.Id
                                      where offer.TblRestaurantId == restaurantID
                                      select new MenuDetails
                                      {
                                          tbl_Offer = offer,
                                          tbl_Cuisine = cuisine,
                                          tbl_Menu = menu

                                      }).ToList();
                    foreach (var item in menudetail)
                    {
                        MenuDetails menuitem = new MenuDetails
                        {
                            tbl_Cuisine = item.tbl_Cuisine,
                            tbl_Menu = item.tbl_Menu,
                            tbl_Offer = item.tbl_Offer
                        };
                        menudetails.Add(menuitem);
                    }
                }
                return menudetails.AsQueryable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<TblRating> GetRestaurantRating(int restaurantID)
        {
            // List<TblRating> restaurant_Rating = new List<TblRating>();
            try
            {
                if (db != null)
                {
                    return (from rating in db.TblRating
                            join restaurant in db.TblRestaurant on
                            rating.TblRestaurantId equals restaurant.Id
                            where rating.TblRestaurantId == restaurantID
                            select new TblRating
                            {
                                Rating = rating.Rating,
                                Comments = rating.Comments,
                                TblRestaurant = restaurant,
                            }).AsQueryable();
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TblRestaurant GetResturantDetails(int restaurantID)
        {
            TblRestaurant resturantInformation = new TblRestaurant();

            try
            {
                if (db != null)
                {
                    resturantInformation = (from restaurant in db.TblRestaurant
                                            join location in db.TblLocation on restaurant.TblLocationId equals location.Id
                                            where restaurant.Id == restaurantID
                                            select new TblRestaurant
                                            {
                                                Id = restaurant.Id,
                                                Name = restaurant.Name,
                                                Address = restaurant.Address,
                                                ContactNo = restaurant.ContactNo,
                                                TblLocation = location,
                                                CloseTime = restaurant.CloseTime,
                                                OpeningTime = restaurant.OpeningTime,
                                                Website = restaurant.Website
                                            }).FirstOrDefault();

                }

                return resturantInformation;

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public IQueryable<TblRestaurantDetails> GetTableDetails(int restaurantID)
        {
            try
            {
                if (db != null)
                {
                    return (from restaurantDetails in db.TblRestaurantDetails
                            join restaurant in db.TblRestaurant
                            on restaurantDetails.TblRestaurantId equals restaurant.Id
                            where restaurantDetails.TblRestaurantId == restaurantID
                            select new TblRestaurantDetails
                            {
                                TableCapacity = restaurantDetails.TableCapacity,
                                TableCount = restaurantDetails.TableCount,
                                TblRestaurant = restaurant
                            }).AsQueryable();

                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<RestaurantSearchDetails> GetRestaurantsBasedOnLocation(LocationDetails location_Details)
        {
            List<RestaurantSearchDetails> restaurants = new List<RestaurantSearchDetails>();
            try
            {
                restaurants = GetRetaurantBasedOnLocationAndName(location_Details);
                return restaurants.AsQueryable();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public IQueryable<RestaurantSearchDetails> GetRestaurantsBasedOnMenu(AddtitionalFeatureForSearch searchDetails)
        {
            List<RestaurantSearchDetails> restaurants = new List<RestaurantSearchDetails>();
            try
            {
                restaurants = GetRestaurantDetailsBasedOnRating(searchDetails);
                return restaurants.AsQueryable();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public IQueryable<RestaurantSearchDetails> SearchForRestaurant(SearchForRestautrant searchDetails)
        {
            List<RestaurantSearchDetails> searchedRestaurantBasedOnRating = new List<RestaurantSearchDetails>();
            searchedRestaurantBasedOnRating = GetRestaurantDetailsBasedOnRating(searchDetails.search);

            List<RestaurantSearchDetails> restaurantsBasedOnLocation = new List<RestaurantSearchDetails>();
            restaurantsBasedOnLocation = GetRetaurantBasedOnLocationAndName(searchDetails.location);

            List<RestaurantSearchDetails> restaurantInfo = new List<RestaurantSearchDetails>();
            restaurantInfo = restaurantsBasedOnLocation.Intersect(searchedRestaurantBasedOnRating, new RestaurantSearchDetailsComparer()).ToList<RestaurantSearchDetails>();
            return restaurantInfo.AsQueryable();
        }


        /// <summary>
        /// Recording the customer rating the restaurants
        /// </summary>
        /// <param name="tblRating"></param>
        public void RestaurantRating(TblRating tblRating)
        {
            //tblRating.UserCreated = ,
            //tblRating.UserModified=,
            tblRating.RecordTimeStampCreated = DateTime.Now;

            db.Set<TblRating>().Add(tblRating);
            db.SaveChanges();

        }
        public TblMenu ItemInStock(int restaurantID, int menuID)
        {
            try
            {
                TblMenu menuObj = new TblMenu();
                if (db != null)
                {
                    //    menuObj = (from m in db.TblMenu
                    //               join offer in db.TblOffer on m.Id equals offer.TblMenuId
                    //               join restaurant in db.TblRestaurantDetails on offer.TblRestaurantId equals restaurant.TblRestaurantId
                    //               where restaurant.TblRestaurantId == restaurantID && m.Id == menuID
                    //               select new TblMenu
                    //               {
                    //                   quantity = m.quantity
                    //               }).FirstOrDefault();                   
                    //}
                    menuObj = (from offer in db.TblOffer
                               join menu in db.TblMenu
                               on offer.TblMenuId equals menu.Id
                               join rest in db.TblRestaurantDetails
                               on offer.TblRestaurantId equals rest.TblRestaurantId
                               where rest.TblRestaurantId == restaurantID && menu.Id == menuID
                               select new TblMenu
                               {
                                   quantity = menu.quantity
                               }).FirstOrDefault();
                }
                return menuObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TblMenu ItemInStock(int menuID)
        {
            try
            {
                TblMenu menuObj = new TblMenu();
                if (db != null)
                {
                    menuObj = (from menu in db.TblMenu
                               where menu.Id == menuID
                               select new TblMenu
                               {
                                   price = menu.price,
                                   quantity = menu.quantity
                               }).FirstOrDefault();
                }
                return menuObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ValidateOffer(int menuID,bool offer)
        {
            try
            {
                if (offer == true)
                {
                    TblMenu menuObj = new TblMenu();
                    if (db != null)
                    {
                        menuObj = (from ofr in db.TblOffer
                                   join menu in db.TblMenu
                                   on ofr.TblMenuId equals menu.Id
                                   where menu.Id == menuID && ofr.FromDate < DateTime.Now &&
                                   ofr.ToDate > DateTime.Now
                                   select new TblMenu
                                   {
                                       quantity = menu.quantity
                                   }).FirstOrDefault();
                        if (menuObj != null)
                        {
                            return true;
                        }
                        return false;
                    }
                    return false;
                }
                else
                {
                    return true;
                } 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Updateitemstock(int quantity, int menuid)
        {
            TblMenu tblobj = new TblMenu();
            tblobj=db.TblMenu.Where(m => m.Id == menuid).FirstOrDefault();
            if (tblobj.quantity > 0)
            {
                int updatedQuantity = tblobj.quantity - quantity;
                tblobj.quantity = updatedQuantity;
                db.Set<TblMenu>().Update(tblobj);
                db.SaveChanges();
                return updatedQuantity;
            }
            return tblobj.quantity;
        }

        #endregion

        #region private methods
        private List<RestaurantSearchDetails> GetRestaurantDetailsBasedOnRating(AddtitionalFeatureForSearch searchList)
        {
            List<RestaurantSearchDetails> restaurants = new List<RestaurantSearchDetails>();
            try
            {
                var restaurantFilter = (from restaurant in db.TblRestaurant
                                        join location in db.TblLocation on restaurant.TblLocationId equals location.Id
                                        join rating in db.TblRating on restaurant.Id equals rating.TblRestaurantId
                                       /// where searchList.rating > Convert.ToInt32(rating.Rating)
                                        orderby rating.Rating descending
                                        select new { TblRestaurant = restaurant, TblLocation = location });

                if (!string.IsNullOrEmpty(searchList.cuisine))
                {
                    restaurantFilter = (from filteredRestaurant in restaurantFilter
                                        join offer in db.TblOffer on filteredRestaurant.TblRestaurant.Id equals offer.TblRestaurantId
                                        join menu in db.TblMenu on offer.TblMenuId equals menu.Id
                                        join cuisine in db.TblCuisine on menu.TblCuisineId equals cuisine.Id
                                        where cuisine.Cuisine.Contains(searchList.cuisine)
                                        select filteredRestaurant).Distinct();
                }
                if (!string.IsNullOrEmpty(searchList.Menu))
                {
                    restaurantFilter = (from filteredRestaurant in restaurantFilter
                                        join offer in db.TblOffer on filteredRestaurant.TblRestaurant.Id equals offer.TblRestaurantId
                                        join menu in db.TblMenu on offer.TblMenuId equals menu.Id
                                        where menu.Item.Contains(searchList.Menu)
                                        select filteredRestaurant).Distinct();
                }

                if (searchList.rating > 0)
                {
                    restaurantFilter = (from filteredRestaurant in restaurantFilter
                                        join rating in db.TblRating on filteredRestaurant.TblRestaurant.Id equals rating.TblRestaurantId
                                        //where rating.Rating.Contains(searchList.rating.ToString())
                                        where Convert.ToInt32(rating.Rating) >= searchList.rating
                                        orderby rating.Rating descending
                                        select filteredRestaurant).Distinct();
                }

                if (searchList.rating == 0)
                {
                    restaurantFilter = (from filteredRestaurant in restaurantFilter
                                        select filteredRestaurant);
                }
                foreach (var item in restaurantFilter)
                {
                    RestaurantSearchDetails restaurant = new RestaurantSearchDetails
                    {
                        restauran_ID = item.TblRestaurant.Id,
                        restaurant_Name = item.TblRestaurant.Name,
                        restaurant_Address = item.TblRestaurant.Address,
                        restaurant_PhoneNumber = item.TblRestaurant.ContactNo,
                        restraurant_Website = item.TblRestaurant.Website,
                        closing_Time = item.TblRestaurant.CloseTime,
                        opening_Time = item.TblRestaurant.OpeningTime,
                        xaxis = item.TblLocation.X.HasValue ? (double)item.TblLocation.X : 0,
                        yaxis = item.TblLocation.Y.HasValue ? (double)item.TblLocation.Y : 0
                    };
                    restaurants.Add(restaurant);
                }
                return restaurants;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private List<RestaurantSearchDetails> GetRetaurantBasedOnLocationAndName(LocationDetails location_Details)
        {
            List<RestaurantSearchDetails> restaurants = new List<RestaurantSearchDetails>();
            try
            {

                var restaurantInfo = (from restaurant in db.TblRestaurant
                                      join location in db.TblLocation on restaurant.TblLocationId equals location.Id
                                      join rating in db.TblRating on restaurant.Id equals rating.TblRestaurantId
                                      orderby rating.Rating descending
                                      select new { TblRestaurant = restaurant, TblLocation = location });

                if (!string.IsNullOrEmpty(location_Details.restaurant_Name))
                {
                    restaurantInfo = restaurantInfo.Where(a => a.TblRestaurant.Name.Contains(location_Details.restaurant_Name));

                }

                if (!(double.IsNaN(location_Details.xaxis)) && !(double.IsNaN(location_Details.yaxis)))
                {
                    foreach (var place in restaurantInfo)
                    {
                        double distance = Distance(location_Details.xaxis, location_Details.yaxis, place.TblLocation.X.HasValue ? (double)place.TblLocation.X : 0, place.TblLocation.Y.HasValue ? (double)place.TblLocation.Y : 0);
                        if (distance < int.Parse(location_Details.distance.ToString()))
                        {
                            RestaurantSearchDetails tblRestaurant = new RestaurantSearchDetails
                            {
                                restauran_ID = place.TblRestaurant.Id,
                                restaurant_Name = place.TblRestaurant.Name,
                                restaurant_Address = place.TblRestaurant.Address,
                                restaurant_PhoneNumber = place.TblRestaurant.ContactNo,
                                restraurant_Website = place.TblRestaurant.Website,
                                closing_Time = place.TblRestaurant.CloseTime,
                                opening_Time = place.TblRestaurant.OpeningTime,
                                xaxis = place.TblLocation.X.HasValue ? (double)place.TblLocation.X : 0,
                                yaxis = place.TblLocation.Y.HasValue ? (double)place.TblLocation.Y : 0
                            };
                            restaurants.Add(tblRestaurant);
                        }
                    }

                }
                return restaurants;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private double Distance(double currentLatitude, double currentLongitude, double latitude, double longitude)
        {
            double theta = currentLatitude - latitude;
            double dist = Math.Sin(GetRadius(currentLatitude)) * Math.Sin(GetRadius(longitude)) + Math.Cos(GetRadius(currentLatitude)) * Math.Cos(GetRadius(latitude)) * Math.Cos(GetRadius(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = (dist * 60 * 1.1515) / 0.6213711922;          //miles to kms
            return (dist);
        }

        private double rad2deg(double dist)
        {
            return (dist * Math.PI / 180.0);
        }

        private double GetRadius(double Latitude)
        {
            return (Latitude * 180.0 / Math.PI);
        }

        public IQueryable<TblMenu> MenuDetails()
        {
            List<TblMenu> tblMenus = db.TblMenu.ToList<TblMenu>();
            return tblMenus.AsQueryable();
        }

        #endregion
    }

    public class RestaurantSearchDetailsComparer : IEqualityComparer<RestaurantSearchDetails>
    {
        public bool Equals(RestaurantSearchDetails details1, RestaurantSearchDetails details2)
        {
            if (details1.restauran_ID == details2.restauran_ID)
            {
                return true;
            }
            return false;
        }
        public int GetHashCode(RestaurantSearchDetails obj)
        {
            return obj.restauran_ID.GetHashCode();
        }
    }
}
