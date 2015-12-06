using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using MySql.Data.Entity;
using Server.DTO;
using Server.Model;

namespace Server.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PlaceService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PlaceService.svc or PlaceService.svc.cs at the Solution Explorer and start debugging.
    public class PlaceService : IPlaceService
    {

        public IEnumerable<PlaceDto> GetPlacesForRestaurant(long restaurantId)
        {
            using (var ctx = new RestAppDbContext())
            {
                return (from p in ctx.Places where p.Restaurant.Id == restaurantId select p).ToList()
                    .Select(PlaceDto.Convert);
            }
        }

        public IEnumerable<PlaceDto> GetPlacesForRestaurantInDate(long restaurantId, DateTime date)
        {
            using (var ctx = new RestAppDbContext())
            {
                return (from p in ctx.Places where p.Restaurant.Id == restaurantId && p.From < date && (p.To > date || p.To == null) select p).ToList()
                    .Select(PlaceDto.Convert);
            }
        }

        public void Add(string seats, DateTime fromDate, long restaurantId, string token)
        {
            using (var ctx = new RestAppDbContext())
            {
                var user = TokenHelper.ValidateToken(token, ctx);

                var restaurant = (from r in ctx.Restaurants where r.Id == restaurantId select r).SingleOrDefault();
                //TODO
                if (restaurant == null) throw new Exception();
                if (restaurant.Owner.Id != user.Id) throw new Exception();
                var correct =
                    !(from p in ctx.Places where p.Restaurant.Id == restaurantId && p.From > fromDate select p).Any();
                //TODO!
                if (!correct) throw new Exception();

                var actualPlace =
                    (from p in ctx.Places where p.Restaurant.Id == restaurantId && p.To == null select p)
                        .SingleOrDefault();

                if (actualPlace != null)
                {
                    actualPlace.To = fromDate;
                }

                ctx.Places.Add(new Place()
                {
                    From = fromDate,
                    Seats = seats,
                    Restaurant = restaurant,
                    To = null
                });

                ctx.SaveChanges();


            }
        }
    }
}