using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTextRPG.Data
{
    internal class Item
    {
        public int Id { get; }
        public string Name { get; }
        public string StatusBonus { get; }
        public string Comment { get; }
        public string Price { get; }

        public Item(int id, string name, string statusBonus, string comment, string price)
        {
            Id = id;
            Name = name;
            StatusBonus = statusBonus;
            Comment = comment;
            Price = price;
        }
        public override string ToString()
        {
            return $"{Id}.{Name} | {StatusBonus} | {Comment} | {Price}";
        }
    }
}
