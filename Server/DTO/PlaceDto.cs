using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Server.Model;

namespace Server.DTO
{
    public class PlaceDto
    {
        public long Id { get; set; }

        public string Seats { get; set; }

        public DateTime From { get; set; }

        public DateTime? To { get; set; }


        public static PlaceDto Convert(Place place)
        {
            return new PlaceDto()
            {
                Id = place.Id,
                Seats = place.Seats,
                From = place.From,
                To = place.To
            };
        }
    }
}