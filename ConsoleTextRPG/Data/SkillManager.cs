using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace ConsoleTextRPG.Data
{
    public class SkillManager
    {
        // 스킬 속성
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ManaCost { get; set; }
        public int Damage { get; set; }
        public string CharacterClass { get; set; }

        private static List<SkillManager> allSkills;

        public static List<SkillManager> LoadAllSkills()
        {
            if (allSkills != null) return allSkills;

            string basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string jsonPath = Path.Combine(basePath, "Json", "Skills.json");

            if (!File.Exists(jsonPath))
            {
                Console.WriteLine($"[오류] Skills.json 파일을 찾을 수 없습니다: {jsonPath}");
                return new List<SkillManager>();
            }

            string json = File.ReadAllText(jsonPath);
            allSkills = JsonConvert.DeserializeObject<List<SkillManager>>(json);

            return allSkills;
        }

        public static List<SkillManager> GetSkillsByJob(string job)
        {
            return LoadAllSkills().Where(s => s.CharacterClass == job).ToList();
        }

        public static void UseSkill(Player player, int skillIndex, Monster target)
        {
            var skills = GetSkillsByJob(player.Job);

            if (skillIndex < 0 || skillIndex >= skills.Count)
            {
                Console.WriteLine("잘못된 스킬 선택입니다.");
                return;
            }

            var skill = skills[skillIndex];

            if (player.Stat.CurrentMp < skill.ManaCost)
            {
                Console.WriteLine("마나가 부족합니다!");
                return;
            }

            player.Stat.CurrentMp -= skill.ManaCost;

            int totalDamage = skill.Damage + player.Stat.TotalAttack;

            Console.WriteLine($"{player.Name}이(가) 스킬 '{skill.Name}'을(를) 사용했습니다! 몬스터에게 {totalDamage}의 피해를 입혔습니다.");

            target.Stat.CurrentHp -= totalDamage;

            if (target.Stat.CurrentHp <= 0)
            {
                target.Stat.CurrentHp = 0;
                Console.WriteLine($"{target.Name} 몬스터가 쓰러졌습니다!");
            }
        }
    }
    }
}
