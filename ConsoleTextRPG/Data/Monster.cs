using ConsoleTextRPG.Managers;
using ConsoleTextRPG.Monsters;
using ConsoleTextRPG.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTextRPG.Data
{
    public abstract class Monster : Character
    {
        public string Name;
        public string Image;
        public int CurHP;
        public int MaxHP;
        public int ATK;
        public int DFP;
        public int Level;
        public int Gold;
        public bool IsDead = false;
    
        public virtual string PrintMonster(int no)
        {
            return Image;
        }
    }
}
