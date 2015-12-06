using Server.Model;

namespace Server.DTO
{
    public class UserDto
    {
        public string Name { get; set; }
        public string Token { get; set; }

        public static UserDto Convert(User user)
        {
            return new UserDto
            {
                Name = user.Name,
                Token = user.Token
            };
        }
    }
}