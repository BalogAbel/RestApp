using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using RestApp.RestaurantService;
using RestApp.Util;

namespace RestApp.Restaurant.AddRestaurant
{
    public class AddRestaurantViewModel: Screen
    {
        private string _restaurantName;

        public string RestaurantName
        {
            get { return _restaurantName; }
            set { _restaurantName = value; NotifyOfPropertyChange(() => RestaurantName);}
        }

        public AddRestaurantViewModel()
        {
            DisplayName = "Add restaurant";
        }

        public async void Add()
        {
            using (var svc = new RestaurantServiceClient())
            {
                await svc.AddRestaurantAsync(RestaurantName, AppData.User.Token);
                TryClose(true);
            }
        }

        public void Cancel()
        {
            TryClose(false);
        }
    }
}
