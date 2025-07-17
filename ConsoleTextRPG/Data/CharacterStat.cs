using System;


namespace ConsoleTextRPG.Data
{
    public class CharacterStat
    {
        public int Level { get; private set; }
        public int MaxHp { get; private set; }
        public int CurrentHp { get; set; }

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
        public int ApplyDamage(int damage)
        {
            int ten = (int)(damage * 0.1f);
            int min = damage- ten;
            int max = damage+ ten;
            int rand= new Random().Next(min, max+1);
            int finalDamage = rand - TotalDefense;
            if (finalDamage < 1) finalDamage = 1; // 최소 1의 데미지는 받도록 보장
            CurrentHp -= finalDamage;
            if (CurrentHp < 0) CurrentHp = 0;

            return finalDamage; // 최종 데미지를 반환
        }

        public int MosApplyDamage(int damage)
        {
            int finalDamage = damage - TotalDefense;
            if (finalDamage < 1) finalDamage = 1; // 최소 1의 데미지는 받도록 보장
            CurrentHp -= finalDamage;
            if (CurrentHp < 0) CurrentHp = 0;
            return finalDamage; // 최종 데미지를 반환
        }

        public void DefecnDamage(int damage)
        {
            int finalDamage = (TotalDefense * 2)- damage;
            if (finalDamage < 1) CurrentHp += finalDamage; //  데미지가 방어력을 넘었다면 차이만큼 데미지입음
            if (CurrentHp < 0) CurrentHp = 0;
        }

        // 기본 스탯을 설정하는 헬퍼 메서드
        public void SetBaseStats(int level, int attack, int defense, int maxHp)
        {
            this.Level = level;
            this.BaseAttack = attack;
            this.BaseDefense = defense;
            this.MaxHp = maxHp;
            this.CurrentHp = maxHp; // 새 스탯 설정 시 체력을 최대로 회복
        }

        // 불러오기 시 저장된 현재 체력을 가져오는 메서드
        public void LoadCurrentHp(int currentHp)
        {
            this.CurrentHp = currentHp;
        }

        // 아이템 장착/해제 시 보너스 스탯을 변경하는 메서드 (추후 제대로 구현)
        public void AddBonusStats(Item item)
        {
            if (item.Type == Item.ItemType.Weapon) AdditionalAttack += item.StatusBonus;
            if (item.Type == Item.ItemType.Armor) AdditionalDefense += item.StatusBonus;
        }

        public void RemoveBonusStats(Item item)
        {
            if (item.Type == Item.ItemType.Weapon) AdditionalAttack -= item.StatusBonus;
            if (item.Type == Item.ItemType.Armor) AdditionalDefense -= item.StatusBonus;
        }
        public void AddHPStats(Item item) //체력함수 추가
        {
            int bonusHp = CurrentHp += item.StatusBonus;
            if (item.Type == Item.ItemType.Potion)
            {
                if (bonusHp > MaxHp)
                {
                    CurrentHp = MaxHp; // 포션 사용 시 최대 체력을 넘지 않도록 보장
                }
                else
                {
                    CurrentHp = bonusHp; // 포션 사용 시 현재 체력에 보너스 체력을 추가
                }

            } 
        }




    }
}
