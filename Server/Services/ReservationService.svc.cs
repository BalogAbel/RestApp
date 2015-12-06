using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Server.DTO;
using Server.Model;

namespace Server.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ReservationService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ReservationService.svc or ReservationService.svc.cs at the Solution Explorer and start debugging.
    public class ReservationService : IReservationService
    {
        public IEnumerable<ReservationDto> GetForRestaurant(long restaurantId)
        {
            using (var ctx = new RestAppDbContext())
            {
                return
                    (from r in ctx.Reservations where r.Place.Restaurant.Id == restaurantId select r).Select(
                        r => ReservationDto.Convert(r));
            }
        }

        public IEnumerable<ReservationDto> GetForUser(string token)
        {
            using (var ctx = new RestAppDbContext())
            {
                var user = TokenHelper.ValidateToken(token, ctx);
                return
                    (from r in ctx.Reservations where r.Guest.Id == user.Id select r).Select(
                        r => ReservationDto.Convert(r));
            }
        }

        public IEnumerable<ReservationDto> GetForPlace(long placeId)
        {
            using (var ctx = new RestAppDbContext())
            {
                return
                    (from r in ctx.Reservations where r.Place.Id == placeId select r).Select(
                        r => ReservationDto.Convert(r));
            }
        }

        public void Add(long placeId, IEnumerable<Tuple<int, int>> seats, DateTime fromDate, DateTime toDate, string token)
        {
            using (var ctx = new RestAppDbContext())
            {
                var user = TokenHelper.ValidateToken(token, ctx);
                if(fromDate >= toDate) throw new Exception();
                var busy = /*(from r in ctx.Reservations where r.Place.Id ==  placeId && r.To > fromDate || r.From -*/
                    false;
                if(busy) throw new Exception();

                var place = (from p in ctx.Places where p.Id == placeId select p).SingleOrDefault();
                if(place == null) throw new Exception();
                if(place.From > fromDate || place.To < toDate) throw new Exception();

                ctx.Reservations.Add(new Reservation()
                {
                    From = fromDate,
                    To = toDate,
                    Place = place,
                    Guest = user,
                    Seats = seats
                });
                ctx.SaveChanges();
            }
        }

        public void Delete(long reservationId, string token)
        {
            using (var ctx = new RestAppDbContext())
            {
                var user = TokenHelper.ValidateToken(token, ctx);
                var reservation = (from r in ctx.Reservations where r.Id == reservationId select r).SingleOrDefault();
                if(reservation == null) throw new Exception();
                if(reservation.Guest.Id != user.Id) throw new Exception();
                ctx.Reservations.Remove(reservation);
                ctx.SaveChanges();
            }

        }
    }
}