using MT.OnlineRestaurant.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.OnlineRestaurant.BusinessLayer
{
    public interface IRestaurantBusiness
    {
        RestaurantInformation GetResturantDetails(int restaurantID);
        IQueryable<RestaurantMenu> GetRestaurantMenus(int restaurantID);
        IQueryable<RestaurantRating> GetRestaurantRating(int restaurantID);
        IQueryable<RestaurantTables> GetTableDetails(int restaunrantID);
        IQueryable<RestaurantInformation> SearchRestaurantByLocation(LocationDetails locationDetails);
        IQueryable<RestaurantInformation> GetRestaurantsBasedOnMenu(AdditionalFeatureForSearch additionalFeatureForSearch);
        IQueryable<RestaurantInformation> SearchForRestaurant(SearchForRestaurant searchDetails);
        IQueryable<MenuList> GetMenuList();

        /// <summary>
        /// Recording the customer rating the restaurants
        /// </summary>
        /// <param name=""></param>
        void RestaurantRating(RestaurantRating restaurantRating);
        int ItemInStock(int restaurantID,int menuID);
        int ItemInStock(int menuID);
        bool Validateoffer(int menuID,bool offer);
        void UpdateItemsinstock(string msg);

    }
}
