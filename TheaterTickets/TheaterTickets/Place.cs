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
            if (json == null)
            {
                throw new ArgumentNullException();
            } 
            row = unchecked((int)json["row"]);
            place = unchecked((int)json["place"]);
            price = unchecked((int)json["price"]);
            free = json["free"];
        }

        public Place(int row, int place)
        {
            this.row = row;
            this.place = place;
            free = false;
            price = 0;
        }

        public string GetTextInfo()
        {
            return string.Format("Ряд: {1}{0}Место: {2}{0}Цена: {3}{0}", Environment.NewLine, row, place, price);
        }
    }
}
