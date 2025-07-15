using ConsoleTextRPG.Data;
using ConsoleTextRPG.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleTextRPG.Monsters
{
    public class Minion : Monster
    {
        public Minion()
        {
            // 몬스터 정보 설정
            Name = "미니언";
            MaxHP = 15;
            Level = 2;
            ATK = 5;
            DFP = 1;
            Gold = 15;


            // 초기화
            CurHP = MaxHP;
        }
        public void PrintMonster<T>(T no, string text, ConsoleColor c)
        {
            Console.ForegroundColor = c;   // 번호 색
            Console.Write($"{no}. ");
            Console.ResetColor();// 기본 색 복원
            Console.WriteLine($"# {no}. {Name}   |   HP : {CurHP}/{MaxHP}");
        }
    }
}
