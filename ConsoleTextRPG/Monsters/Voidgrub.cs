using ConsoleTextRPG.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTextRPG.Monsters
{
    public class Voidgrub : Monster
    {
        public Voidgrub()
        {
            // 몬스터 정보 설정
            Name = "공허충";
            MaxHP = 10;
            Level = 3;
            ATK = 9;
            DFP = 1;
            Gold = 20;

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
