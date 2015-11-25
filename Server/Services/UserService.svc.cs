using System;
using System.Linq;
using System.ServiceModel;
using Server.Model;
using Server.Services.Exceptions;

namespace Server.Services
{
    public class UserService : IUserService
    {
        public void RegisterUser(string userName, string password, bool asOwner = false)
        {
            using (var ctx = new RestAppDbContext())
            {
                var user = from u in ctx.Users where u.Name == userName select u;
                if (user.Count() != 0)
                {
                    throw new FaultException<AlreadyRegisteredException>(new AlreadyRegisteredException());
                }
                User newUser = null;
                if (asOwner)
                {
                    U
                }
                ctx.Users.Add(new User
                {
                    Name = userName,
                    Password = password
                });
                ctx.SaveChanges();
            }
        }

        public string Login(string userName, string password)
        {
            using (var ctx = new RestAppDbContext())
            {
                var user =
                    (from u in ctx.Users where u.Name == userName && u.Password == password select u).FirstOrDefault();
                if (user == null)
                {
                    throw new FaultException<BadLoginCredentialsException>(new BadLoginCredentialsException());
                }
                user.Token = new Guid();
                ctx.SaveChanges();
                return user.Token.ToString();
            }
        }
    }
}