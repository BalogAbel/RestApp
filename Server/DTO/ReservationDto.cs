using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Server.Model;

namespace Server.DTO
{
    public class ReservationDto
    {
        public long Id { get; set; }

        public string Guest { get; set; }

        public RestaurantDto Restaurant { get; set; }

        public IEnumerable<Tuple<int, int>> Seats { get; set; }

        public static ReservationDto Convert(Reservation reservation)
        {
            return new ReservationDto()
            {
                Id = reservation.Id,
                Guest = reservation.Guest.Name,
                Restaurant = RestaurantDto.Convert(reservation.Place.Restaurant),
                Seats = reservation.Seats
            };
        }
    }
}