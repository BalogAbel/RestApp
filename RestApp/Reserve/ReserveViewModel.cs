using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Caliburn.Micro;
using RestApp.PlaceService;
using RestApp.ReservationService;
using RestApp.RestaurantService;
using RestApp.Util;

namespace RestApp.Reserve
{
    [Export(typeof (ReserveViewModel))]
    public class ReserveViewModel : Screen
    {
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

        private RestaurantDto _selectedRestaurant;

        public RestaurantDto SelectedRestaurant
        {
            get { return _selectedRestaurant; }
            set
            {
                _selectedRestaurant = value;
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
            set
            {
                _fromDate = value;
                NotifyOfPropertyChange(() => FromDate);
            }
        }

        private DateTime _toDate;

        public DateTime ToDate
        {
            get { return _toDate; }
            set
            {
                _toDate = value;
                NotifyOfPropertyChange(() => ToDate);
            }
        }

        #endregion

        private Grid _gridPlace;
        private byte[,] _place;
        private List<Tuple<int, int>> _seats;

        public ReserveViewModel()
        {
            Init();
        }

        public async void Init()
        {
            using (var restaurantSvc = new RestaurantServiceClient())
            using (var reservationSvc = new ReservationServiceClient())
            {
                Restaurants =
                    new BindableCollection<RestaurantDto>(await restaurantSvc.GetMyRestaurantsAsync(AppData.User.Token));
                Reservations =
                    new BindableCollection<ReservationDto>(await reservationSvc.GetForUserAsync(AppData.User.Token));
            }
        }

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);
            _gridPlace = ((ReserveView) view).GridPlace;
        }

        private async void InitGrid()
        {
            _seats = new List<Tuple<int, int>>();
            ConvertPlace(SelectedPlace.Seats);
            ReservationDto[] reservations;
            using (var svc = new ReservationServiceClient())
            {
                reservations = (await svc.GetForPlaceAsync(SelectedPlace.Id)).Where(r =>
                    r.From >= FromDate && r.From <= ToDate ||
                    r.To >= FromDate && r.To <= ToDate ||
                    r.From <= FromDate && r.To >= ToDate
                    ).ToArray();
            }
            foreach (var reservation in reservations)
            {
                foreach (var seat in reservation.Seats)
                {
                    _place[seat.Item1, seat.Item2] = 3;
                }
            }
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
                    border.MouseLeftButtonUp += ReserveSeat;
                    ;

                    var style = _place[i, j] == 0
                        ? "Empty"
                        : _place[i, j] == 1 ? "Seat" : _place[i, j] == 2 ? "Table" : "Reserved";
                    border.Style = _gridPlace.FindResource(style) as Style;
                    _gridPlace.Children.Add(border);
                }
                first = false;
            }
        }


        public void SaveDate()
        {
            if (SelectedPlace != null) InitGrid();
        }

        public void SaveReservation()
        {
            using (var svc = new ReservationServiceClient())
            {
                svc.Add(SelectedPlace.Id, _seats.ToArray(), FromDate, ToDate, AppData.User.Token);
                InitGrid();
                RefreshReservations();
            }
        }

        private async void RefreshPlaces()
        {
            using (var svc = new PlaceServiceClient())
            {
                Places = new BindableCollection<PlaceDto>((await  svc.GetPlacesForRestaurantAsync(SelectedRestaurant.Id)).ToList());
                var lastPlace = (from p in Places where p.To == null select p).SingleOrDefault();
                FromDate = lastPlace?.From.AddDays(1) ?? DateTime.Now;
                ToDate = FromDate.AddHours(2);
            }
        }


        private async void RefreshReservations()
        {
            using (var svc = new ReservationServiceClient())
            {
                Reservations = new BindableCollection<ReservationDto>((await svc.GetForPlaceAsync(SelectedPlace.Id)).ToList());
                var lastPlace = (from p in Places where p.To == null select p).SingleOrDefault();
                FromDate = lastPlace?.From.AddDays(1) ?? DateTime.Now;
                ToDate = FromDate.AddHours(2);
            }
        }

        private void ConvertPlace(string place)
        {
            _place = new byte[50, 50];
            var rows = place.Split(';');
            for (var i = 0; i < rows.Length; i++)
            {
                var cols = rows[i].Split(',');
                for (var j = 0; j < cols.Length; j++)
                {
                    _place[i, j] = byte.Parse(cols[j]);
                }
            }
        }


        private void ReserveSeat(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            if (border == null) return;

            var row = (int) border.GetValue(Grid.RowProperty);
            var column = (int) border.GetValue(Grid.ColumnProperty);
            var position = _place[row, column];

            if (position == 1)
            {
                border.Style = _gridPlace.FindResource("Reserving") as Style;
                _place[row, column] = 11;
                _seats.Add(new Tuple<int, int>(row, column));
                return;
            }
            if (position == 11)
            {
                border.Style = _gridPlace.FindResource("Seat") as Style;
                _place[row, column] = 1;
                _seats.Remove(new Tuple<int, int>(row, column));
            }
        }

    }
}