using ConsoleTextRPG.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTextRPG.Monsters
{
    public class SiegeMinion : Monster
    {
        public SiegeMinion()
        {
            // 몬스터 정보 설정
            Name = "대포미니언";
            MaxHP = 25;
            Level = 5;
            ATK = 8;
            DFP = 3;
            Gold = 50;

            // 초기화
            CurHP = MaxHP;
        }
        public override void PrintMonster(int no, ConsoleColor c)
        {
            Console.ResetColor();// 기본 색 복원
            Console.ForegroundColor = c;   // 번호 색
            Console.Write($"# {no}. ");
            Console.ResetColor();// 기본 색 복원
            Console.WriteLine($"{Name} | HP : {CurHP}/{MaxHP}");
        }
    }
}
