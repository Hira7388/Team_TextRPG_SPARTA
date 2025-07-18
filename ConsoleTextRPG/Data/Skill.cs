using System;
using ConsoleTextRPG.Data;

namespace ConsoleTextRPG.Managers
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Damage { get; set; }
        public float CriticalChance { get; set; }
        public string JobRestriction { get; set; }
        public string Effect { get; set; }
        public int Cooldown { get; set; } // 최대 쿨타임 (턴 수)
        public int CurrentCooldown { get; set; } // 현재 남은 쿨타임

        public Skill(int id, string name, int damage, float criticalChance, int cooldown, string jobRestriction = null, string effect = null)
        {
            Id = id;
            Name = name;
            Damage = damage;
            CriticalChance = criticalChance;
            Cooldown = cooldown;
            CurrentCooldown = 0; // 초기 쿨타임 0
            JobRestriction = jobRestriction;
            Effect = effect;
        }

        public bool Use(Character user, Character target)
        {
            Player player = user as Player;

            if (player != null && JobRestriction != null && player.Job != JobRestriction)
            {
                Console.WriteLine($"{user.Name}은(는) 이 스킬을 사용할 수 없습니다!");
                return false;
            }

            if (CurrentCooldown > 0)
            {
                Console.WriteLine($"{Name}은(는) 쿨타임 중입니다! (남은 턴: {CurrentCooldown})");
                return false;
            }

            Console.WriteLine($"{user.Name}이(가) {Name} 스킬을 사용했습니다!");
            CurrentCooldown = Cooldown;

            Random rand = new Random();
            bool isCritical = rand.NextDouble() < CriticalChance;
            int finalDamage = Damage;

            if (isCritical)
            {
                finalDamage *= 2;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{Name} 치명타 발동!! 데미지 2배!");
                Console.ResetColor();
            }

            else if (Effect == "Heal" && player != null)
            {
                int healAmount = Damage;
                player.Stat.CurrentHp = Math.Min(player.Stat.MaxHp, player.Stat.CurrentHp + healAmount);
                Console.WriteLine($"{user.Name}이(가) {healAmount}만큼 회복했습니다! (현재 HP: {player.Stat.CurrentHp})");
                return true;
            }

            target.TakeDamage(finalDamage);
            if (target.Stat.CurrentHp <= 0)
            {
                Console.WriteLine($"{target.Name}이(가) 스킬에 의해 쓰러졌습니다!");
                QuestManager.Instance.OnMonsterKilled(target.Name); // 퀘스트 알림도 여기서 처리 가능
            }

            return true;
        }


        public void ReduceCooldown()
        {
            if (CurrentCooldown > 0)
            {
                CurrentCooldown--;
            }
        }

        public Skill Clone()
        {
            return (Skill)this.MemberwiseClone();
        }
    }
}