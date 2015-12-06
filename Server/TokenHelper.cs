using System.Linq;
using System.ServiceModel;
using Server.Exceptions;
using Server.Model;

namespace Server
{
    public class TokenHelper
    {
        public static User ValidateToken(string token, RestAppDbContext ctx)
        {
            if (token == null)
                throw new FaultException<BadLoginCredentialsException>(new BadLoginCredentialsException());

            var user = (from u in ctx.Users where u.Token == token select u).FirstOrDefault();
            if (user == null)
                throw new FaultException<BadLoginCredentialsException>(new BadLoginCredentialsException());

            return user;
        }

        public static User ValidateToken(string token)
        {
            using (var ctx = new RestAppDbContext())
            {
                return ValidateToken(token, ctx);
            }
        }
    }
}