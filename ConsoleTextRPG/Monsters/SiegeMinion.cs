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
        // 몬스터 이미지 설정
        public override string PrintMonster(int no)
        {
            string[] artLines = new[]
           {
            $"# {no}. {Name}",
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
