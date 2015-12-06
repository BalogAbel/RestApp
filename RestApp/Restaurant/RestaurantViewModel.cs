using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using RestApp.RestaurantService;
using RestApp.Util;

namespace RestApp.Restaurant
{
    [Export(typeof (RestaurantViewModel))]
    public class RestaurantViewModel : Screen
    {
        public string Asd { get { return "asd"; } }

        private IObservableCollection<RestaurantDto> _restaurants;

        public IObservableCollection<RestaurantDto> Restaurants
        {
            get { return _restaurants; }
            set { _restaurants = value; NotifyOfPropertyChange(() => Restaurants);}
        }

        private RestaurantDto _selecteRestaurant;

        public RestaurantDto SelectedRestaurant
        {
            get { return  _selecteRestaurant; }
            set {  _selecteRestaurant = value; NotifyOfPropertyChange(() => SelectedRestaurant);}
        }


        public RestaurantViewModel()
        {
            using (var svc = new RestaurantServiceClient())
            {
                Restaurants =
                    new BindableCollection<RestaurantDto>(svc.GetMyRestaurants(AppData.User.Token));
            }
        }
    }
}