using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTextRPG.Data
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Itemtype { get; set; }
        public int StatusBonus { get; set; }
        public string Comment { get; set; }
        public int Price { get; set; }

        public Item(int id, string name, string itemType, int statusBonus, string comment, int price)
        {
            Id = id;
            Name = name;
            Itemtype = itemType;
            StatusBonus = statusBonus;
            Comment = comment;
            Price = price;
        }
        public override string ToString()
        {
            return $"{Id}.{Name} | {Itemtype} + {StatusBonus} | {Comment} | {Price} G";
        }
    }
}
