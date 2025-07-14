using System;


namespace ConsoleTextRPG.Data
{
    public class CharacterStat
    {
        public int Level { get; private set; }
        public int MaxHp { get; private set; }
        public int CurrentHp { get; private set; }

        // 초기 능력치
        public int BaseAttack { get; private set; }
        public int BaseDefense { get; private set; }

        // 추가 능력치
        public int AdditionalAttack { get; private set; }
        public int AdditionalDefense { get; private set; }

        // 최종 능력치
        public int TotalAttack => BaseAttack + AdditionalAttack;
        public int TotalDefense => BaseDefense + AdditionalDefense;

        // 사망 여부를 알려주는 bool형 변수 프로퍼티
        public bool IsDead => CurrentHp <= 0;

        public CharacterStat(int level, int atk, int def, int hp)
        {
            Level = level;
            BaseAttack = atk;
            BaseDefense = def;
            MaxHp = hp;
            CurrentHp = hp; // 처음에는 체력이 가득 찬 상태
        }

        // 데미지를 계산하고 적의 공격 데미지와 내 방어력을 계산해서 HP를 변경하는 로직
        public void ApplyDamage(int damage)
        {
            int finalDamage = damage - TotalDefense;
            if (finalDamage < 1) finalDamage = 1; // 최소 1의 데미지는 받도록 보장

            CurrentHp -= finalDamage;
            if (CurrentHp < 0) CurrentHp = 0;
        }
    }
}
