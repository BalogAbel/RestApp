using System;
using System.Linq;
using System.ServiceModel;
using Server.DTO;
using Server.Exceptions;
using Server.Model;

namespace Server.Services
{
    public class UserService : IUserService
    {
        public UserDto RegisterUser(string userName, string password)
        {
            using (var ctx = new RestAppDbContext())
            {
                var user = from u in ctx.Users where u.Name == userName select u;
                if (user.Count() != 0)
                {
                    throw new FaultException<AlreadyRegisteredException>(new AlreadyRegisteredException());
                }
                var newUser = new User
                {
                    Name = userName,
                    Password = password
                };
                ctx.Users.Add(newUser);
                ctx.SaveChanges();
                return UserDto.Convert(newUser);
            }
        }

        public UserDto Login(string userName, string password)
        {
            using (var ctx = new RestAppDbContext())
            {
                var user1 =
                    (from u in ctx.Users where u.Name == userName && u.Password == password select u);
                var user = user1.SingleOrDefault();
                if (user == null)
                {
                    throw new FaultException<BadLoginCredentialsException>(new BadLoginCredentialsException());
                }
                user.Token = Guid.NewGuid().ToString();
                ctx.SaveChanges();
                return UserDto.Convert(user);
            }
        }
    }
}