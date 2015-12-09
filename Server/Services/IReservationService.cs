using System;
using System.Collections.Generic;
using System.ServiceModel;
using Server.DTO;
using Server.Exceptions;

namespace Server.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ReservationService" in both code and config file together.
    [ServiceContract]
    public interface IReservationService
    {

        [OperationContract]
        IEnumerable<ReservationDto> GetForRestaurant(long restaurantId);

        [OperationContract]
        [FaultContract(typeof(BadLoginCredentialsException))]
        IEnumerable<ReservationDto> GetForUser(string token);

        [OperationContract]
        IEnumerable<ReservationDto> GetForPlace(long placeId);

        [OperationContract]
        [FaultContract(typeof(BadLoginCredentialsException))]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(DateOrderException))]
        [FaultContract(typeof(SeatsAreBusyException))]
        [FaultContract(typeof(PlaceDateException))]
        void Add(long placeId, IEnumerable<Tuple<int, int>> seats, DateTime fromDate, DateTime toDate, string token);

        [OperationContract]
        [FaultContract(typeof(BadLoginCredentialsException))]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(NotAuthorizedException))]
        void Delete(long reservationId, string token);
    }
}