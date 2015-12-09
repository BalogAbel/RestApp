using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Caliburn.Micro;
using RestApp.PlaceService;
using RestApp.ReservationService;
using RestApp.Restaurant.AddRestaurant;
using RestApp.RestaurantService;
using RestApp.Util;
using RestaurantDto = RestApp.RestaurantService.RestaurantDto;

namespace RestApp.Restaurant
{
    [Export(typeof (RestaurantViewModel))]
    public class RestaurantViewModel : Screen
    {
        private byte[,] _place = new byte[50, 50];
        private Grid _gridPlace;

        #region Properties

        private IObservableCollection<RestaurantDto> _restaurants;

        public IObservableCollection<RestaurantDto> Restaurants
        {
            get { return _restaurants; }
            set
            {
                _restaurants = value;
                NotifyOfPropertyChange(() => Restaurants);
            }
        }

        private RestaurantDto _selecteRestaurant;

        public RestaurantDto SelectedRestaurant
        {
            get { return _selecteRestaurant; }
            set
            {
                _selecteRestaurant = value;
                NotifyOfPropertyChange(() => SelectedRestaurant);
                RefreshPlaces();
            }
        }


        private IObservableCollection<PlaceDto> _places;

        public IObservableCollection<PlaceDto> Places
        {
            get { return _places; }
            set
            {
                _places = value;
                NotifyOfPropertyChange(() => Places);
            }
        }

        private PlaceDto _selectedPlace;

        public PlaceDto SelectedPlace
        {
            get { return _selectedPlace; }
            set
            {
                _selectedPlace = value;
                NotifyOfPropertyChange(() => SelectedPlace);
                RefreshReservations();
            }
        }

        private IObservableCollection<ReservationDto> _reservations;

        public IObservableCollection<ReservationDto> Reservations
        {
            get { return _reservations; }
            set
            {
                _reservations = value;
                NotifyOfPropertyChange(() => Reservations);
            }
        }

        private ReservationDto _selectedReservation;

        public ReservationDto SelectedReservation
        {
            get { return _selectedReservation; }
            set
            {
                _selectedReservation = value;
                NotifyOfPropertyChange(() => SelectedReservation);
            }
        }

        private DateTime _fromDate;

        public DateTime FromDate
        {
            get { return _fromDate; }
            set { _fromDate = value; NotifyOfPropertyChange(() => FromDate); }
        }
        

        #endregion

        public RestaurantViewModel()
        {
            Init();
        }

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);
            _gridPlace = ((RestaurantView) view).GridPlace;
            InitGrid();
        }

        private void InitGrid()
        {
            var first = true;
                _gridPlace.ColumnDefinitions.Clear();
                _gridPlace.RowDefinitions.Clear();
                _gridPlace.Width = _place.GetLength(0)*10;
                _gridPlace.Height = _place.GetLength(1)*10;

            for (var i = 0; i < _place.GetLength(0); i++)
            {
                _gridPlace.ColumnDefinitions.Add(new ColumnDefinition());
                for (var j = 0; j < _place.GetLength(1); j++)
                {
                    if (first) _gridPlace.RowDefinitions.Add(new RowDefinition());
                    var border = new Border();
                    border.SetValue(Grid.RowProperty, i);
                    border.SetValue(Grid.ColumnProperty, j);
                    border.MouseLeftButtonUp += ChangeToSeat;
                    border.MouseRightButtonUp += ChangeToTable;
                    border.Style = _gridPlace.FindResource("Empty") as Style;
                    _gridPlace.Children.Add(border);
                }
                first = false;
            }

        }


        public void Init()
        {
            using (var svc = new RestaurantServiceClient())
            {
                Restaurants =
                    new BindableCollection<RestaurantDto>(svc.GetMyRestaurants(AppData.User.Token));
            }
        }

        public void AddRestaurant()
        {
            var result = IoC.Get<IWindowManager>().ShowDialog(new AddRestaurantViewModel());
            if (result.HasValue && result.Value) Init();
        }

        private void RefreshPlaces()
        {
            using (var svc = new PlaceServiceClient())
            {
                Places = new BindableCollection<PlaceDto>(svc.GetPlacesForRestaurant(SelectedRestaurant.Id).ToList());
                var lastPlace = (from p in Places where p.To == null select p).SingleOrDefault();
                FromDate = lastPlace?.From.AddDays(1) ?? DateTime.Now;
            }
        }

        private void RefreshReservations()
        {
            using (var svc = new ReservationServiceClient())
            {
                var res = svc.GetForPlace(SelectedPlace.Id);
                Reservations = new BindableCollection<ReservationDto>(res);
            }
        }


        public void SavePlace()
        {
            var strPlace = "";
            for (var i = 0; i < _place.GetLength(0); i++)
            {
                for (var j = 0; j < _place.GetLength(1); j++)
                {
                    strPlace += _place[i,j].ToString();
                    if (j != _place.GetLength(1)-1) strPlace += ",";

                }

                if (i != _place.GetLength(0)-1) strPlace += ";";
            }
            using (var svc = new PlaceServiceClient())
            {
                svc.Add(strPlace, FromDate, SelectedRestaurant.Id, AppData.User.Token);
            }
            RefreshPlaces();
            _place = new byte[50,50];
            InitGrid();
        }

        #region PlaceHelpers

        private void ChangeToSeat(object sender, RoutedEventArgs e)
        {
            var border = sender as Border;
            if (border == null) return;

            var row = (int) border.GetValue(Grid.RowProperty);
            var column = (int) border.GetValue(Grid.ColumnProperty);

            if (_place[row, column] != 1)
            {
                border.Style = _gridPlace.FindResource("Seat") as Style;
                _place[row, column] = 1;
            }
            else
            {
                border.Style = _gridPlace.FindResource("Empty") as Style;
                _place[row, column] = 0;
            }
        }

        private void ChangeToTable(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            if (border == null) return;

            var row = (int) border.GetValue(Grid.RowProperty);
            var column = (int) border.GetValue(Grid.ColumnProperty);

            if (_place[row, column] != 2)
            {
                border.Style = _gridPlace.FindResource("Table") as Style;
                _place[row, column] = 2;
            }
            else
            {
                border.Style = _gridPlace.FindResource("Empty") as Style;
                _place[row, column] = 0;
            }
        }
        #endregion

    }
}