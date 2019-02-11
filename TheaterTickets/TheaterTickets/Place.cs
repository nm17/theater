using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheaterTickets
{
    public class Place
    {
        public readonly int row;
        public readonly int place;
        public readonly int price;
        public readonly bool free;

        public Place(Dictionary<string, dynamic> json)
        {
            row = unchecked((int)json["row"]);
            place = unchecked((int)json["place"]);
            price = unchecked((int)json["price"]);
            free = json["free"];
        }

        public Place(int row_, int place_)
        {
            row = row_;
            place = place_;
            free = false;
            price = 0;
        }
    }
}
