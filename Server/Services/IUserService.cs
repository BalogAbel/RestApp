using System.ServiceModel;
using Server.DTO;
using Server.Exceptions;

namespace Server.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUserService" in both code and config file together.
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        [FaultContract(typeof (AlreadyRegisteredException))]
        UserDto RegisterUser(string username, string password);

        [OperationContract]
        [FaultContract(typeof (BadLoginCredentialsException))]
        UserDto Login(string username, string password);
    }
}