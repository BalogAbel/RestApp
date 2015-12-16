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

        public string Restaurant { get; set; }

        public IEnumerable<Tuple<int, int>> Seats { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }


        public static ReservationDto Convert(Reservation reservation)
        {
            return new ReservationDto()
            {
                Id = reservation.Id,
                Guest = reservation.Guest.Name,
                Restaurant = reservation.Place.Restaurant.Name,
                Seats = reservation.Seats.Select(seat => new Tuple<int, int>(seat.Column, seat.Row)).ToList(),
                From = reservation.From,
                To = reservation.To
            };
        }
    }
}