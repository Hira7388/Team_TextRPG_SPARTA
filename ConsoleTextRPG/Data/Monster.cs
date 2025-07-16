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
        public virtual void PrintMonster( int no, ConsoleColor c)
        {
            Console.ForegroundColor = c;   // 번호 색
            Console.Write($"# {no}. ");
            Console.ResetColor();// 기본 색 복원
            Console.WriteLine($"Lv.{Level} | {Name} | HP : {CurHP}/{MaxHP}");
        }
        public virtual void PrintMonster(ConsoleColor c)
        {
            Console.WriteLine($"Lv.{Level} | {Name} | HP : {CurHP}/{MaxHP}");
        }
        public virtual void PrintMonster()
        {
            Console.WriteLine($"Lv.{Level} | {Name} | HP : {CurHP}/{MaxHP}");
        }
    }
}
