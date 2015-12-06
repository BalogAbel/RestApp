using Server.Model;

namespace Server.DTO
{
    public class RestaurantDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }


        public static RestaurantDto Convert(Restaurant restaurant)
        {
            return new RestaurantDto
            {
                Id = restaurant.Id,
                Name = restaurant.Name
            };
        }
    }
}