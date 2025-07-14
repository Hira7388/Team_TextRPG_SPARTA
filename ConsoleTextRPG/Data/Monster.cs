using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTextRPG.Data
{
    internal abstract class Monster : Character
    {
        public string name;
        public string image;
        public int curHp;
        public int maxHp;
        public int atk;
        public int dfp;
    }
}
