using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTextRPG.Data
{
    public enum QuestType
    {
        Hunting,
        Equip,
        LevelUp
    }
    public class Quest // 퀘스트 자체의 핵심 데이터
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public QuestType Type { get; set; }
        public string TargetName { get; set; }
        public int TargetCount { get; set; }
        public int RewardGold { get; set; }
        public List<int> RewardItemIds { get; set; }
    }
}
