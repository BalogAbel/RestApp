using System;
using System.Collections.Generic;
using System.ServiceModel;
using Server.DTO;
using Server.Exceptions;

namespace Server.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IRestaurantService" in both code and config file together.
    [ServiceContract]
    public interface IRestaurantService
    {

        [OperationContract]
        [FaultContract(typeof(BadLoginCredentialsException))]
        List<RestaurantDto> GetAllRestaurant(string token);

        [OperationContract]
        [FaultContract(typeof(BadLoginCredentialsException))]
        List<RestaurantDto> GetMyRestaurants(string token);

        [OperationContract]
        [FaultContract(typeof(BadLoginCredentialsException))]
        void AddRestaurant(string name, string token);

        [OperationContract]
        [FaultContract(typeof(BadLoginCredentialsException))]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(NotAuthorizedException))]
        [FaultContract(typeof(ConcurrencyException))]
        void EditRestaurant(long id, string name, Guid version, string token);

        [OperationContract]
        [FaultContract(typeof(BadLoginCredentialsException))]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(NotAuthorizedException))]
        void DeleteRestaurant(RestaurantDto restaurant, string token);
    }
}