using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Server.DTO;
using Server.Exceptions;
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
                    (from r in ctx.Reservations where r.Place.Restaurant.Id == restaurantId select r).ToList().Select(
                        ReservationDto.Convert);
            }
        }

        public IEnumerable<ReservationDto> GetForUser(string token)
        {
            using (var ctx = new RestAppDbContext())
            {
                var user = TokenHelper.ValidateToken(token, ctx);
                return
                    (from r in ctx.Reservations where r.Guest.Id == user.Id select r).ToList().Select(
                        ReservationDto.Convert);
            }
        }

        public IEnumerable<ReservationDto> GetForPlace(long placeId)
        {
            using (var ctx = new RestAppDbContext())
            {
                return
                    (from r in ctx.Reservations where r.Place.Id == placeId select r).ToList().Select(
                        ReservationDto.Convert);
            }
        }

        public void Add(long placeId, IEnumerable<Tuple<int, int>> seats, DateTime fromDate, DateTime toDate,
            string token)
        {
            using (var ctx = new RestAppDbContext())
            {
                var user = TokenHelper.ValidateToken(token, ctx);
                var place = (from p in ctx.Places where p.Id == placeId select p).SingleOrDefault();
                if (place == null) throw new FaultException<NotFoundException>(new NotFoundException());
                if (fromDate >= toDate) throw new FaultException<DateOrderException>(new DateOrderException());
                ;
                var busy = /*(from r in ctx.Reservations where r.Place.Id ==  placeId && r.To > fromDate || r.From -*/
                    false;
                if (busy) throw new FaultException<SeatsAreBusyException>(new SeatsAreBusyException());

                if (place.From > fromDate || place.To < toDate)
                    throw new FaultException<PlaceDateException>(new PlaceDateException());
                ;

                ctx.Reservations.Add(new Reservation
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
                if (reservation == null) throw new FaultException<NotFoundException>(new NotFoundException());
                if (reservation.Guest.Id != user.Id)
                    throw new FaultException<NotAuthorizedException>(new NotAuthorizedException());
                ctx.Reservations.Remove(reservation);
                ctx.SaveChanges();
            }
        }
    }
}