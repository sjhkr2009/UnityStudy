namespace AbstractFactory {
    public enum ChampionIndex {
        Ezreal,
        Lulu
    }
    
    public static class CharacterFactory {
        public static IChampionFactory CreateFactory(ChampionIndex championIndex)
        {
            switch (championIndex) {
                case ChampionIndex.Ezreal:
                    return new EzrealFactory();
                case ChampionIndex.Lulu:
                    return new LuluFactory();
            }

            return null;
        }
    }
    
    public interface IChampionFactory {
        // 1. 팩토리가 각각의 요소를 생성하는 인터페이스를 가지고 있는 경우 
        ChampionIndex GetChampionIndex { get; }
        CharacterStat GetOriginStat { get; }
        QSkillHandler GetSkillQ { get; }
        WSkillHandler GetSkillW { get; }
        ESkillHandler GetSkillE { get; }
        
        // 2. 팩토리가 직접 추상 객체를 생성하여 반환하는 경우
        Character Create();
    }

    public class EzrealFactory : IChampionFactory {
        public ChampionIndex GetChampionIndex => ChampionIndex.Ezreal;

        public CharacterStat GetOriginStat => new CharacterStat() {
            health = 530,
            cost = 375,
            attackDamage = 60,
            armor = 24,
            magicResistance = 30,
            movementSpeed = 325
        };
        public QSkillHandler GetSkillQ => new MysticShot();
        public WSkillHandler GetSkillW => new EssenceFlux();
        public ESkillHandler GetSkillE => new ArcaneShift();
        
        public Character Create() => new Character(this);
    }
    public class LuluFactory : IChampionFactory {
        public ChampionIndex GetChampionIndex => ChampionIndex.Lulu;
        public CharacterStat GetOriginStat => new CharacterStat() {
            health = 525,
            cost = 350,
            attackDamage = 47,
            armor = 29,
            magicResistance = 30,
            movementSpeed = 330
        };
        public QSkillHandler GetSkillQ => new Glitterlance();
        public WSkillHandler GetSkillW => new Whimsy();
        public ESkillHandler GetSkillE => new HelpPix();
        
        public Character Create() => new Character(this);
    }
}
