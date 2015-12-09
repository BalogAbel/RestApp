using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Server.DTO;
using Server.Exceptions;
using Server.Model;

namespace Server.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RestaurantService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select RestaurantService.svc or RestaurantService.svc.cs at the Solution Explorer and start debugging.
    public class RestaurantService : IRestaurantService
    {
        public List<RestaurantDto> GetAllRestaurant(string token)
        {
            using (var ctx = new RestAppDbContext())
            {
                TokenHelper.ValidateToken(token, ctx);
                return (from r in ctx.Restaurants select RestaurantDto.Convert(r)).ToList();
            }
        }

        public List<RestaurantDto> GetMyRestaurants(string token)
        {
            using (var ctx = new RestAppDbContext())
            {
                var user = TokenHelper.ValidateToken(token, ctx);
                var restaurants = (from r in ctx.Restaurants where r.Owner.Id == user.Id select r).ToList();
                return restaurants.Select(r => RestaurantDto.Convert(r)).ToList();
            }
        } 

        public void AddRestaurant(string name, string token)
        {
            using (var ctx = new RestAppDbContext())
            {
                var user = TokenHelper.ValidateToken(token, ctx);
                ctx.Restaurants.Add(new Restaurant
                {
                    Name = name,
                    Owner = user
                });
                ctx.SaveChanges();
            }
        }

        public void EditRestaurant(long id, string name, string token)
        {
            using (var ctx = new RestAppDbContext())
            {
                var user = TokenHelper.ValidateToken(token, ctx);
                var restaurantEntity = (from r in ctx.Restaurants where r.Id == id select r).FirstOrDefault();
                if (restaurantEntity == null) throw new FaultException<NotFoundException>(new NotFoundException());
                if (restaurantEntity.Owner.Id != user.Id) throw new FaultException<NotAuthorizedException>(new NotAuthorizedException());
                restaurantEntity.Name = name;
                ctx.SaveChanges();
            }
        }

        public void DeleteRestaurant(RestaurantDto restaurant, string token)
        {
            using (var ctx = new RestAppDbContext())
            {
                var user = TokenHelper.ValidateToken(token, ctx);
                var restaurantEntity = (from r in ctx.Restaurants where r.Id == restaurant.Id select r).FirstOrDefault();
                if (restaurantEntity == null) throw new FaultException<NotFoundException>(new NotFoundException());
                if (restaurantEntity.Owner.Id != user.Id) throw new FaultException<NotAuthorizedException>(new NotAuthorizedException());
               /* ctx.Reservations.RemoveRange(restaurantEntity.Reservations);
                foreach (var reservation in restaurantEntity.Reservations)
                {
                    reservation.Guest.Reservations.Remove(reservation);
                }
                user.Restaurants.Remove(restaurantEntity);*/
                ctx.Restaurants.Remove(restaurantEntity);
                ctx.SaveChanges();
            }
        }


    }
}