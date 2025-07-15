using ConsoleTextRPG.Data;
using ConsoleTextRPG.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
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
            CurHP = MaxHP;
            // 전투 상호작용을 위한 스탯 초기화 설정
            this.Stat = new CharacterStat(Level, ATK, DFP, MaxHP);
        }
        public override void PrintMonster(int no, ConsoleColor c)
        {
            Console.ForegroundColor = c;   // 번호 색
            Console.Write($"# {no}. ");
            Console.ResetColor();// 기본 색 복원
            Console.WriteLine($"Lv.{Level} | ,{Name} | HP : {CurHP}/{MaxHP}");
        }
        public override void PrintMonster()
        {
            Console.WriteLine($"Lv.{Level} | ,{Name} | HP : {CurHP}/{MaxHP}");
        }
        public override void PrintMonster(ConsoleColor c)
        {
            Console.ForegroundColor = c;   // 번호 색
            Console.WriteLine($"Lv.{Level} | ,{Name} | HP : {CurHP}/{MaxHP}");
        }
    }
}
