using System.ServiceModel;
using Server.Services.Exceptions;

namespace Server.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUserService" in both code and config file together.
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        [FaultContract(typeof (AlreadyRegisteredException))]
        void RegisterUser(string userName, string password);

        [OperationContract]
        [FaultContract(typeof (BadLoginCredentialsException))]
        string Login(string userName, string password);
    }
}