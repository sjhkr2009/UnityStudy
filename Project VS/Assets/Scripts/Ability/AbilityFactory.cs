public static class AbilityFactory {
    public static AbilityBase Create(AbilityIndex abilityIndex) {
        AbilityBase ability = abilityIndex switch {
            AbilityIndex.WeaponAutoGun => new FireBulletWeapon(),
            AbilityIndex.WeaponSpinAround => new SpinnerWeapon(),
            AbilityIndex.WeaponRandomAreaStrike => new RandomAreaStrikeWeapon(),
            AbilityIndex.SkillFireball => new FireballSkill(),
            AbilityIndex.SkillChargeExplosion => new ChargeExplosionSkill(),
            AbilityIndex.SkillPeriodicLaser => new PeriodicLaserSkill(),
            _ => null
        };
        if (ability == null) Debugger.Error($"[ItemFactory.Create] Cannot find {abilityIndex} Implementation!");
        
        return ability;
    }
}
