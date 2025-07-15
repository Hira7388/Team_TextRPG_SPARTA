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
        public SiegeMinion(Monster ms) : base(ms)
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
        public void PrintMonsterInfo(int i)
        {
            // 이미지 설정
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢤⣖⡄⠀⠀⠀⠀⠀");
            sb.AppendLine("⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣜⣞⢎⡷⡀⠀⠀⠀⠀");
            sb.AppendLine("⠀⠀⠀⠀⠀⠀⠀⢀⣄⡶⣝⡮⣟⡾⣝⢶⡀⠀⠀⠀");
            sb.AppendLine("⠀⠀⠀⠀⠀⠀⢠⣞⡗⠿⢘⢯⣗⡯⡯⣗⠗⠀⠀⠀");
            sb.AppendLine("⠀⠀⠀⠀⠀⠀⠠⡹⡮⡯⣮⡪⣷⣻⣽⢽⢠⢤⣀⡀");
            sb.AppendLine("⠀⢀⣔⢤⢤⢔⢮⡳⣝⢽⢮⣻⡺⣾⣺⣏⢧⢳⢼⡂");
            sb.AppendLine("⠀⢸⠪⢿⣹⣵⢺⢽⢽⣝⣷⣳⣯⣳⣻⢮⡫⡧⣷⠁");
            sb.AppendLine("⢀⣔⣾⣱⣟⡎⣗⡽⡻⣞⣗⡟⡞⠈⠀⠳⣝⢝⠇⠀");
            sb.AppendLine("⠘⠚⠞⠚⣷⢧⡧⡇⣏⣞⢮⢯⠀⠀⠀⠀⠀⠀⠀⠀");
            sb.AppendLine("⠀⠀⠀⠀⠈⠫⠯⠺⠚⠊⠋⠁⠀⠀⠀⠀⠀⠀⠀⠀");
            sb.AppendLine($"\n {i}. {Name}");
            sb.AppendLine($"HP : {CurHP}");
            Image = sb.ToString();
        }
    }
}
