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
        public override string PrintMonster(int no)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"# {no}. {Name}   |   HP : {CurHP}/{MaxHP}");
            return sb.ToString(); ;
        }
    }
}
