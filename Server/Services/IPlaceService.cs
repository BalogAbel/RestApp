using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Server.DTO;
using Server.Exceptions;

namespace Server.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPlaceService" in both code and config file together.
    [ServiceContract]
    public interface IPlaceService
    {

        [OperationContract]
        IEnumerable<PlaceDto> GetPlacesForRestaurant(long restaurantId);

        [OperationContract]
        IEnumerable<PlaceDto> GetPlacesForRestaurantInDate(long restaurantId, DateTime date);

        [OperationContract]
        [FaultContract(typeof(BadLoginCredentialsException))]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(NotAuthorizedException))]
        [FaultContract(typeof(NotNewestPlaceException))]
        void Add(string seats, DateTime fromDate, long restaurantId, string token);
    }
}
