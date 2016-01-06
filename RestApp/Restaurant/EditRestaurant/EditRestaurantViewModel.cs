using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using RestApp.RestaurantService;
using RestApp.Util;

namespace RestApp.Restaurant.EditRestaurant
{
    public class EditRestaurantViewModel: Screen
    {
        private string _restaurantName;

        public string RestaurantName
        {
            get { return _restaurantName; }
            set { _restaurantName = value; NotifyOfPropertyChange(() => RestaurantName);}
        }

        private RestaurantDto _restaurant;

        public EditRestaurantViewModel(RestaurantDto restaurant)
        {
            DisplayName = "Edit restaurant";
            _restaurant = restaurant;
            RestaurantName = _restaurant.Name;
        }

        public async void Edit()
        {
            using (var svc = new RestaurantServiceClient())
            {
                try
                {
                    await svc.EditRestaurantAsync(_restaurant.Id, RestaurantName, _restaurant.Version, AppData.User.Token);
                }
                catch (FaultException<ConcurrencyException> e)
                {
                    MessageBox.Show("The restaurant has been modified by someone else, your changes has been reverted", "Concurrency error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                TryClose(true);
            }
        }
        
        public void Cancel()
        {
            TryClose(false);
        }
    }
}
