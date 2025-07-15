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

        public override string PrintMonster(int no)
        {
            string[] artLines = new[]
           {
            $"# {no}. {Name} ",
            $"# HP : {CurHP}/{MaxHP}",
             };
            StringBuilder sb = new StringBuilder();
            int targetWidth = artLines.Max(line => line.Length);

            // 각 행을 PadRight로 동일 너비로 맞춘 뒤 추가
            foreach (var line in artLines)
            {
                sb.AppendLine(line.PadRight(targetWidth));
            }
            return sb.ToString(); 
        }
    }
}
